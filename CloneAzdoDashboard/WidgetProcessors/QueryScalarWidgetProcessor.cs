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
      if (widget.settings == null)
      {
        WriteWarning("Skipped: settings == null");
        return;
      }
      WorkItemQuery targetQuery;
      var settings = JsonConvert.DeserializeObject<QueryScalarWidgetSettings>(widget.settings);
      targetQuery = QueryTools.CopyQuery(new CopyQueryParameters
      {
        QueryId = settings.queryId,
        QueryReplacements = appConfig.Queries,
      });
      settings.queryId = targetQuery.id;
      settings.queryName = targetQuery.name;
      widget.settings = JsonConvert.SerializeObject(settings);
    }
  }
}
