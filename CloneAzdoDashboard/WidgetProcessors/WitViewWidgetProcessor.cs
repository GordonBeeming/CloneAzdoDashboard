using CloneAzdoDashboard.Tools;
using CloneAzdoDashboard.Tools.Parameters;
using CloneAzdoDashboard.WidgetProcessors.WidgetySettings;
using Newtonsoft.Json;

namespace CloneAzdoDashboard.WidgetProcessors
{
  public class WitViewWidgetProcessor : BaseWidgetProcessor
  {
    public override string ContributionId => "ms.vss-mywork-web.Microsoft.VisualStudioOnline.MyWork.WitViewWidget";

    public override void Run(DashboardInfo_Widget1 widget, AppConfig appConfig)
    {
      WorkItemQuery targetQuery;
      var querySettings = JsonConvert.DeserializeObject<WitViewWidgetSettings>(widget.settings);
      targetQuery = QueryTools.CopyQuery(new CopyQueryParameters
      {
        QueryId = querySettings.query.queryId,
        QueryReplacements = appConfig.Queries,
      });
      querySettings.query.queryId = targetQuery.id;
      querySettings.query.queryName = targetQuery.name;
      widget.settings = JsonConvert.SerializeObject(querySettings);
    }
  }
}
