namespace CloneAzdoDashboard.WidgetProcessors.WidgetySettings
{
  public class AnalyticsSprintBurndownWidgetSettings
  {
    public bool totalScopeTrendlineEnabled { get; set; }
    public bool completedWorkEnabled { get; set; }
    public bool stackByWorkItemTypeEnabled { get; set; }
    public bool showNonWorkingDays { get; set; }
    public Aggregation aggregation { get; set; }
    public Workitemtypefilter workItemTypeFilter { get; set; }
    //public Timeperiodconfiguration timePeriodConfiguration { get; set; }
    public Team team { get; set; }
    public string iterationId { get; set; }
    //public string iterationPath { get; set; }
    public bool isMinimalChart { get; set; }
    public bool isLightboxChart { get; set; }
    public bool isLegacy { get; set; }
    public bool isCustomized { get; set; }
  }

  public class Aggregation
  {
    public int identifier { get; set; }
    public string settings { get; set; }
  }

  public class Workitemtypefilter
  {
    public string identifier { get; set; }
    public string settings { get; set; }
  }

  public class Timeperiodconfiguration
  {
    public string startDate { get; set; }
    public string endDate { get; set; }
  }

  public class Team
  {
    public string teamId { get; set; }
    public string projectId { get; set; }
  }

}
