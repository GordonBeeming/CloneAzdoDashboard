using System.Collections.Generic;
using CloneAzdoDashboard.Config;
using CloneAzdoDashboard.Tools.Parameters;

namespace CloneAzdoDashboard
{
  public class AppConfig
  {
    public MigrationModes Mode { get; set; } = MigrationModes.Dashboard;

    public string SourceTeamProjectBaseUri { get; set; }
    public string TargetTeamProjectBaseUri { get; set; }
    public string SourcePatKey { get; set; }
    public string TargetPatKey { get; set; }

    public string SourceTeamName { get; set; }
    public string SourceDashboardName { get; set; }
    public bool SourceAsProject { get; set; }

    public string TargetTeamName { get; set; }
    public string TargetDashboardName { get; set; }
    public bool TargetAsProject { get; set; }

    public bool UpdateQueriesOnly { get; set; }
    public bool DeleteTargetDashboardIfExists { get; set; }
    public bool NullSettingsWhereNoSupportedProcessorExists { get; set; } = true;

    public QueryReplacementParameters Queries { get; set; } = new QueryReplacementParameters();

    public List<FindAndReplace> MarkdownFindAndReplace { get; set; } = new List<FindAndReplace>();

    public BuildsConfig Builds { get; set; } = new BuildsConfig();

    public ReposConfig Repos { get; set; } = new ReposConfig();
  }

}
