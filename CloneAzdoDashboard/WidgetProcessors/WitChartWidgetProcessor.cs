using CloneAzdoDashboard.Tools;
using CloneAzdoDashboard.Tools.Parameters;
using CloneAzdoDashboard.WidgetProcessors.WidgetySettings;
using Newtonsoft.Json;

namespace CloneAzdoDashboard.WidgetProcessors
{
  public class WitChartWidgetProcessor : BaseWidgetProcessor
  {
    public override string ContributionId => "ms.vss-dashboards-web.Microsoft.VisualStudioOnline.Dashboards.WitChartWidget";

    public override void Run(DashboardInfo_Widget1 widget, AppConfig appConfig)
    {
      WorkItemQuery targetQuery;
      var settings = JsonConvert.DeserializeObject<WitChartWidgetSettings>(widget.settings);
      targetQuery = QueryTools.CopyQuery(new CopyQueryParameters
      {
        QueryId = settings.groupKey,
        QueryReplacements = appConfig.Queries,
      });
      settings.groupKey = targetQuery.id;
      settings.transformOptions.filter = targetQuery.id;
      widget.settings = JsonConvert.SerializeObject(settings);
    }
  }
}
