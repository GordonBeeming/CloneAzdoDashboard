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
      var settings = JsonConvert.DeserializeObject<WitViewWidgetSettings>(widget.settings);
      targetQuery = QueryTools.CopyQuery(new CopyQueryParameters
      {
        QueryId = settings.query.queryId,
        QueryReplacements = appConfig.Queries,
      });
      settings.query.queryId = targetQuery.id;
      settings.query.queryName = targetQuery.name;
      widget.settings = JsonConvert.SerializeObject(settings);
    }
  }
}
