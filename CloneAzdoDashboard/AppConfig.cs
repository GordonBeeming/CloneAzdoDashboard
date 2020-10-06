using CloneAzdoDashboard.Tools.Parameters;

namespace CloneAzdoDashboard
{
  public class AppConfig
  {
    public string SourceTeamProjectBaseUri { get; set; }
    public string TargetTeamProjectBaseUri { get; set; }
    public string SourcePatKey { get; set; }
    public string TargetPatKey { get; set; }
    public string SourceTeamName { get; set; }
    public string SourceDashboardName { get; set; }
    public string TargetTeamName { get; set; }
    public string TargetDashboardName { get; set; }
    public bool DeleteTargetDashboardIfExists { get; set; }
    //public bool UseSourceWhenAvailable { get; set; }

    public QueryReplacementParameters Queries { get; set; }
  }

}
