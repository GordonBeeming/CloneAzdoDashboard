using System.Collections.Generic;

namespace CloneAzdoDashboard.WidgetProcessors.WidgetySettings
{
  public class CycleTimeWidgetSettings
  {
    public CycleTimeWidgetSettings_DataSettings dataSettings { get; set; }
    public string lastArtifactName { get; set; }
  }

  public class CycleTimeWidgetSettings_DataSettings
  {
    public string project { get; set; }
    public List<string> teamIds { get; set; } = new List<string>();
    public CycleTimeWidgetSettings_WitSelector witSelector { get; set; }
    public string swimLane { get; set; }
    public List<CycleTimeWidgetSettings_FieldFilter> fieldFilters { get; set; } = new List<CycleTimeWidgetSettings_FieldFilter>();
    public CycleTimeWidgetSettings_FieldFilterTimePeriod timePeriod { get; set; }
  }

  public class CycleTimeWidgetSettings_WitSelector
  {
    public string identifier { get; set; }
    public string settings { get; set; }
  }

  public class CycleTimeWidgetSettings_FieldFilterTimePeriod
  {
    public string identifier { get; set; }
    public int settings { get; set; }
  }

  public class CycleTimeWidgetSettings_FieldFilter
  {
    public string fieldName { get; set; }
    public string queryOperation { get; set; }
    public string queryValue { get; set; }
  }

}
