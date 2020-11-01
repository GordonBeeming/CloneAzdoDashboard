namespace CloneAzdoDashboard.WidgetProcessors.WidgetySettings
{

  public class BuildHistogramWidgetSettings
  {
    public BuildHistogramWidgetSettings_Builddefinition buildDefinition { get; set; }
    public object fullBranchName { get; set; }
  }

  public class BuildHistogramWidgetSettings_Builddefinition
  {
    public string name { get; set; }
    public int id { get; set; }
    public int type { get; set; }
    public string uri { get; set; }
    public string projectId { get; set; }
  }


}
