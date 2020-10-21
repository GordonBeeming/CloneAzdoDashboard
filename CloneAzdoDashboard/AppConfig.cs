using System.Collections.Generic;
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
    public bool SourceAsProject { get; set; }
    public string TargetTeamName { get; set; }
    public string TargetDashboardName { get; set; }
    public bool TargetAsProject { get; set; }
    public bool DeleteTargetDashboardIfExists { get; set; }
    //public bool UseSourceWhenAvailable { get; set; }
    public bool UpdateQueriesOnly { get; set; }

    public QueryReplacementParameters Queries { get; set; }

    public List<FindAndReplace> MarkdownFindAndReplace { get; set; } = new List<FindAndReplace>();
  }

}
