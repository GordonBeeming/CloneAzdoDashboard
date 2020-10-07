namespace CloneAzdoDashboard.WidgetProcessors.WidgetySettings
{
  public class WitChartWidgetSettings
  {
    public string scope { get; set; }
    public string groupKey { get; set; }
    public string title { get; set; }
    public string chartType { get; set; }
    public WitChartWidgetSettings_Transformoptions transformOptions { get; set; }
    public object[] userColors { get; set; }
    public string lastArtifactName { get; set; }
  }

  public class WitChartWidgetSettings_Transformoptions
  {
    public string filter { get; set; }
    public string groupBy { get; set; }
    public WitChartWidgetSettings_Orderby orderBy { get; set; }
    public WitChartWidgetSettings_Measure measure { get; set; }
    public object historyRange { get; set; }
  }

  public class WitChartWidgetSettings_Orderby
  {
    public string direction { get; set; }
    public string propertyName { get; set; }
  }

  public class WitChartWidgetSettings_Measure
  {
    public string aggregation { get; set; }
    public string propertyName { get; set; }
  }

}
