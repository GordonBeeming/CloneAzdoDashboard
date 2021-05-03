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
      if (widget.settings == null)
      {
        WriteWarning("Skipped: settings == null");
        return;
      }
      string sourceProjectName = TfsStatic.GetTeamProjectName(true);
      string sourceTeamName = appConfig.SourceTeamName;
      string targetProjectName = TfsStatic.GetTeamProjectName(false);
      string targetTeamName = appConfig.TargetTeamName;
      WorkItemQuery targetQuery;
      var settings = JsonConvert.DeserializeObject<WitViewWidgetSettings>(widget.settings);
      targetQuery = QueryTools.CopyQuery(new CopyQueryParameters
      {
        QueryId = settings.query.queryId,
        QueryReplacements = appConfig.Queries,
      }, sourceProjectName, sourceTeamName, targetProjectName, targetTeamName);
      settings.query.queryId = targetQuery.id;
      settings.query.queryName = targetQuery.name;
      widget.settings = JsonConvert.SerializeObject(settings);
    }
  }
}
