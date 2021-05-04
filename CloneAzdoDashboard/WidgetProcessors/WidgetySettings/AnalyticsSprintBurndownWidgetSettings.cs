using System;

namespace CloneAzdoDashboard.WidgetProcessors.WidgetySettings
{
  public class AnalyticsSprintBurndownWidgetSettings
  {
    public bool totalScopeTrendlineEnabled { get; set; }
    public bool completedWorkEnabled { get; set; }
    public bool stackByWorkItemTypeEnabled { get; set; }
    public bool showNonWorkingDays { get; set; }
    public AnalyticsSprintBurndownWidgetSettings_Aggregation aggregation { get; set; }
    public AnalyticsSprintBurndownWidgetSettings_WorkItemTypeFilter workItemTypeFilter { get; set; }
    public AnalyticsSprintBurndownWidgetSettings_TimePeriodConfiguration timePeriodConfiguration { get; set; }
    public AnalyticsSprintBurndownWidgetSettings_Team team { get; set; }
    public string iterationId { get; set; }
    public string iterationPath { get; set; }
    public bool isMinimalChart { get; set; }
    public bool isLightboxChart { get; set; }
    public bool isLegacy { get; set; }
    public bool isCustomized { get; set; }
  }

  public class AnalyticsSprintBurndownWidgetSettings_Aggregation
  {
    public int identifier { get; set; }
    public string settings { get; set; }
  }

  public class AnalyticsSprintBurndownWidgetSettings_WorkItemTypeFilter
  {
    public string identifier { get; set; }
    public string settings { get; set; }
  }

  public class AnalyticsSprintBurndownWidgetSettings_TimePeriodConfiguration
  {
    public DateTime? startDate { get; set; }
    public DateTime? endDate { get; set; }
  }

  public class AnalyticsSprintBurndownWidgetSettings_Team
  {
    public string teamId { get; set; }
    public string projectId { get; set; }
  }

}
