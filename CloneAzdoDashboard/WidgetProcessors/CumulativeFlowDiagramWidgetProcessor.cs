using System;
using System.Collections.Generic;
using System.Linq;
using CloneAzdoDashboard.WidgetProcessors.WidgetySettings;
using Newtonsoft.Json;

namespace CloneAzdoDashboard.WidgetProcessors
{
  public class CumulativeFlowDiagramWidgetProcessor : BaseWidgetProcessor
  {
    public override string ContributionId => "ms.vss-dashboards-web.Microsoft.VisualStudioOnline.Dashboards.CumulativeFlowDiagramWidget";

    private static TeamList _sourceTeamList = null;
    private static TeamList _targetTeamList = null;

    public override void Run(DashboardInfo_Widget1 widget, AppConfig appConfig)
    {
      if (widget.settings == null)
      {
        WriteWarning("Skipped: settings == null");
        return;
      }
      var settings = JsonConvert.DeserializeObject<CumulativeFlowDiagramWidgetSettings>(widget.settings);

      if (_sourceTeamList == null)
      {
        _sourceTeamList = TfsStatic.GetTeams(true, TfsStatic.GetTeamProjectId(true));
      }
      if (_targetTeamList == null)
      {
        _targetTeamList = TfsStatic.GetTeams(false, TfsStatic.GetTeamProjectId(false));
      }

      var sourceTeam = _sourceTeamList.value.FirstOrDefault(o => o.id == settings.chartDataSettings.team);
      var sourceTeamName = sourceTeam?.name;
      if (string.IsNullOrEmpty(sourceTeamName))
      {
        WriteWarning($"Skipped: Can't find a team in the source project with an id of '{settings.chartDataSettings.team}'.");
        return;
      }
      var targetTeamName = sourceTeamName;
      var sourceIsMigratingTeam = sourceTeamName.Equals(appConfig.SourceTeamName, StringComparison.OrdinalIgnoreCase);
      if (sourceIsMigratingTeam)
      {
        targetTeamName = appConfig.TargetTeamName;
      }
      var targetTeamId = _targetTeamList.value.FirstOrDefault(o => o.name == targetTeamName)?.id;
      if (string.IsNullOrEmpty(targetTeamId))
      {
        if (sourceTeam.name.Equals($"{sourceTeam.projectName} Team"))
        {
          var targetDefaultTeam = $"{TfsStatic.GetTeamProjectName(false)} Team";
          var targetTeam = _targetTeamList.value.FirstOrDefault(o => o.name.Equals(targetDefaultTeam, System.StringComparison.InvariantCultureIgnoreCase));
          if (targetTeam == null)
          {
            WriteWarning($"Skipped: Can't find a team in the target project with the name of '{targetDefaultTeam}'.");
            return;
          }
          WriteWarning($"Default team detected in the source, using default team in target: '{targetTeam.name}'.");
          targetTeamId = targetTeam.id;
        }
        else
        {
          WriteWarning($"Skipped: Can't find a team in the target project with the name of '{targetTeamName}'.");
          return;
        }
      }

      var sourceBoards = TfsStatic.GetTeamBoards(true, sourceTeam.id);
      var sourceBoard = sourceBoards.value.FirstOrDefault(o => o.id.Equals(settings.chartDataSettings.board, StringComparison.OrdinalIgnoreCase));
      if (sourceBoard == null)
      {
        WriteWarning($"Can't find source board id '{settings.chartDataSettings.board}'.");
      }
      else
      {
        var targetBoards = TfsStatic.GetTeamBoards(false, targetTeamId);
        var targetBoard = targetBoards.value.FirstOrDefault(o => o.name.Equals(sourceBoard.name, StringComparison.OrdinalIgnoreCase));
        if (targetBoard == null)
        {
          WriteWarning($"Can't find target board named '{sourceBoard.name}'.");
        }
        else
        {
          settings.chartDataSettings.board = targetBoard.id;

          var sourceFullBoard = TfsStatic.GetTeamBoard(true, sourceTeam.id, sourceBoard.id);
          var targetFullBoard = TfsStatic.GetTeamBoard(false, targetTeamId, targetBoard.id);

          var desiredBoardColumnIds = new List<string>();
          foreach (var desiredBoardColumnId in settings.chartDataSettings.desiredBoardColumnIds)
          {
            var sourceColumn = sourceFullBoard.columns.FirstOrDefault(o => o.id.Equals(desiredBoardColumnId, StringComparison.OrdinalIgnoreCase));
            if (sourceColumn == null)
            {
              WriteWarning($"Can't find source column id '{desiredBoardColumnId}'.");
              continue;
            }
            var targetColumn = targetFullBoard.columns.FirstOrDefault(o => o.name.Equals(sourceColumn.name, StringComparison.OrdinalIgnoreCase));
            if (targetColumn == null)
            {
              WriteWarning($"Can't find target column named '{sourceColumn.name}'.");
              continue;
            }
            desiredBoardColumnIds.Add(targetColumn.id);
          }
          settings.chartDataSettings.desiredBoardColumnIds = desiredBoardColumnIds;
        }
      }

      settings.chartDataSettings.project = TfsStatic.GetTeamProjectId(false);
      settings.chartDataSettings.team = targetTeamId;
      settings.lastArtifactName?.Replace(appConfig.SourceTeamName, appConfig.TargetTeamName);

      widget.settings = JsonConvert.SerializeObject(settings);
    }
  }
}
