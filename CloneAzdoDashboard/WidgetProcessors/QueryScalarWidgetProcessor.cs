using CloneAzdoDashboard.Tools;
using CloneAzdoDashboard.Tools.Parameters;
using CloneAzdoDashboard.WidgetProcessors.WidgetySettings;
using Newtonsoft.Json;

namespace CloneAzdoDashboard.WidgetProcessors
{
  public class QueryScalarWidgetProcessor : BaseWidgetProcessor
  {
    public override string ContributionId => "ms.vss-dashboards-web.Microsoft.VisualStudioOnline.Dashboards.QueryScalarWidget";

    public override void Run(DashboardInfo_Widget1 widget, AppConfig appConfig)
    {
      WorkItemQuery targetQuery;
      var querySettings = JsonConvert.DeserializeObject<QueryScalarWidgetSettings>(widget.settings);
      targetQuery = QueryTools.CopyQuery(new CopyQueryParameters
      {
        QueryId = querySettings.queryId,
        QueryReplacements = appConfig.Queries,
      });
      querySettings.queryId = targetQuery.id;
      querySettings.queryName = targetQuery.name;
      widget.settings = JsonConvert.SerializeObject(querySettings);
    }
  }
}
