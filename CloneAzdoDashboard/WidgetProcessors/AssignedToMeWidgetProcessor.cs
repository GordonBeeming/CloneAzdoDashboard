namespace CloneAzdoDashboard.WidgetProcessors
{
  public class AssignedToMeWidgetProcessor : BaseWidgetProcessor
  {
    public override string ContributionId => "ms.vss-dashboards-web.Microsoft.VisualStudioOnline.Dashboards.AssignedToMeWidget";

    public override void Run(DashboardInfo_Widget1 widget, AppConfig appConfig)
    {
      // nothing is required
    }
  }
}
