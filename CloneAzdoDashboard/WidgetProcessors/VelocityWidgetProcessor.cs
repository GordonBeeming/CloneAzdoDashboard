using System;
using System.Linq;
using CloneAzdoDashboard.WidgetProcessors.WidgetySettings;
using Newtonsoft.Json;

namespace CloneAzdoDashboard.WidgetProcessors
{
  public class VelocityWidgetProcessor : BaseWidgetProcessor
  {
    public override string ContributionId => "ms.vss-dashboards-web.Microsoft.VisualStudioOnline.Dashboards.VelocityWidget";

    private static TeamList _sourceTeamList = null;
    private static TeamList _targetTeamList = null;

    public override void Run(DashboardInfo_Widget1 widget, AppConfig appConfig)
    {
      if (widget.settings == null)
      {
        WriteWarning("Skipped: settings == null");
        return;
      }
      var settings = JsonConvert.DeserializeObject<VelocityWidgetSettings>(widget.settings);

      if (_sourceTeamList == null)
      {
        _sourceTeamList = TfsStatic.GetTeams(true, TfsStatic.GetTeamProjectId(true));
      }
      if (_targetTeamList == null)
      {
        _targetTeamList = TfsStatic.GetTeams(false, TfsStatic.GetTeamProjectId(false));
      }

      var sourceTeam = _sourceTeamList.value.FirstOrDefault(o => o.id == settings.teamId);
      var sourceTeamName = sourceTeam?.name;
      if (string.IsNullOrEmpty(sourceTeamName))
      {
        WriteWarning($"Skipped: Can't find a team in the source project with an id of '{settings.teamId}'.");
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

      settings.projectId = TfsStatic.GetTeamProjectId(false);
      settings.teamId = targetTeamId;
      settings.lastArtifactName?.Replace(appConfig.SourceTeamName, appConfig.TargetTeamName);

      widget.settings = JsonConvert.SerializeObject(settings);
    }
  }
}
