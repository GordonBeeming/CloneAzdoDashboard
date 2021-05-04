namespace CloneAzdoDashboard.WidgetProcessors.WidgetySettings
{
  public class VelocityWidgetSettings
  {
    public string projectId { get; set; }
    public string teamId { get; set; }
    public VelocityWidgetSettings_WorkItemTypeFilter workItemTypeFilter { get; set; }
    public VelocityWidgetSettings_Aggregation aggregation { get; set; }
    public int numberOfIterations { get; set; }
    public int plannedWorkDelay { get; set; }
    public int lateWorkDelay { get; set; }
    public string lastArtifactName { get; set; }
  }

  public class VelocityWidgetSettings_WorkItemTypeFilter
  {
    public string identifier { get; set; }
    public string settings { get; set; }
  }

  public class VelocityWidgetSettings_Aggregation
  {
    public int identifier { get; set; }
    public string settings { get; set; }
  }


}
