namespace CloneAzdoDashboard.WidgetProcessors.WidgetySettings
{
  public class WitViewWidgetSettings
  {
    public WitViewWidgetSettings_Query query { get; set; }
    public WitViewWidgetSettings_Selectedcolumn[] selectedColumns { get; set; }
    public string lastArtifactName { get; set; }
  }

  public class WitViewWidgetSettings_Query
  {
    public string queryId { get; set; }
    public string queryName { get; set; }
  }

  public class WitViewWidgetSettings_Selectedcolumn
  {
    public string name { get; set; }
    public string referenceName { get; set; }
  }

}
