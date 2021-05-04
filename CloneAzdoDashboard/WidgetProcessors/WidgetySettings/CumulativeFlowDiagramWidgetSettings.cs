using System.Collections.Generic;

namespace CloneAzdoDashboard.WidgetProcessors.WidgetySettings
{
  public class CumulativeFlowDiagramWidgetSettings
  {
    public CumulativeFlowDiagramWidgetSettings_ChartDataSettings chartDataSettings { get; set; }
    public string themeName { get; set; }
    public string lastArtifactName { get; set; }
  }

  public class CumulativeFlowDiagramWidgetSettings_ChartDataSettings
  {
    public string project { get; set; }
    public string team { get; set; }
    public string board { get; set; }
    public string boardLane { get; set; }
    public CumulativeFlowDiagramWidgetSettings_TimePeriod timePeriod { get; set; }
    public List<string> desiredBoardColumnIds { get; set; } = new List<string>();
  }

  public class CumulativeFlowDiagramWidgetSettings_TimePeriod
  {
    public string identifier { get; set; }
    public int settings { get; set; }
  }
}
