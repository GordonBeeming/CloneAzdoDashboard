using System.Collections.Generic;

namespace CloneAzdoDashboard.WidgetProcessors.WidgetySettings
{
  public class LeadTimeWidgetSettings
  {
    public LeadTimeWidgetSettings_DataSettings dataSettings { get; set; }
    public string lastArtifactName { get; set; }
  }

  public class LeadTimeWidgetSettings_DataSettings
  {
    public string project { get; set; }
    public List<string> teamIds { get; set; } = new List<string>();
    public LeadTimeWidgetSettings_WitSelector witSelector { get; set; }
    public object swimLane { get; set; }
    public object[] fieldFilters { get; set; }
    public LeadTimeWidgetSettings_TimePeriod timePeriod { get; set; }
  }

  public class LeadTimeWidgetSettings_WitSelector
  {
    public string identifier { get; set; }
    public string settings { get; set; }
  }

  public class LeadTimeWidgetSettings_TimePeriod
  {
    public string identifier { get; set; }
    public int settings { get; set; }
  }

}
