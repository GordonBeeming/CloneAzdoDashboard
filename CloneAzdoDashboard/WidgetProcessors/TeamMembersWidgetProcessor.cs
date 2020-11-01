using CloneAzdoDashboard.WidgetProcessors.WidgetySettings;
using Newtonsoft.Json;
using System.Linq;

namespace CloneAzdoDashboard.WidgetProcessors
{
  public class TeamMembersWidgetProcessor : BaseWidgetProcessor
  {
    public override string ContributionId => "ms.vss-dashboards-web.Microsoft.VisualStudioOnline.Dashboards.TeamMembersWidget";

    private static TeamList sourceTeamList = null;
    private static TeamList targetTeamList = null;

    public override void Run(DashboardInfo_Widget1 widget, AppConfig appConfig)
    {
      var settings = JsonConvert.DeserializeObject<TeamMembersWidgetSettings>(widget.settings);

      if (sourceTeamList == null)
      {
        sourceTeamList = TfsStatic.GetTeams(true, TfsStatic.GetTeamProjectId(true));
      }
      if (targetTeamList == null)
      {
        targetTeamList = TfsStatic.GetTeams(false, TfsStatic.GetTeamProjectId(false));
      }

      var sourceTeam = sourceTeamList.value.FirstOrDefault(o => o.id == settings.teamId);
      var sourceTeamName = sourceTeam?.name;
      if (string.IsNullOrEmpty(sourceTeamName))
      {
        WriteWarning($"Skipped: Can't find a team in the source project with an id of '{settings.teamId}'.");
        return;
      }
      var targetTeamId = targetTeamList.value.FirstOrDefault(o => o.name == sourceTeamName)?.id;
      if (string.IsNullOrEmpty(targetTeamId))
      {
        if (sourceTeam.name.Equals($"{sourceTeam.projectName} Team"))
        {
          var targetDefaultTeam = $"{TfsStatic.GetTeamProjectName(false)} Team";
          var targetTeam = targetTeamList.value.FirstOrDefault(o => o.name.Equals(targetDefaultTeam, System.StringComparison.InvariantCultureIgnoreCase));
          if (targetTeam == null)
          {
            WriteWarning($"Skipped: Can't find a team in the target project with the name of '{sourceTeamName}'.");
            return;
          }
          WriteWarning($"Default team detected in the source, using default team in target: '{targetTeam.name}'.");
          targetTeamId = targetTeam.id;
        }
        else
        {
          WriteWarning($"Skipped: Can't find a team in the target project with the name of '{sourceTeamName}'.");
          return;
        }
      }

      settings.teamId = targetTeamId;

      widget.settings = JsonConvert.SerializeObject(settings);
    }
  }
}
