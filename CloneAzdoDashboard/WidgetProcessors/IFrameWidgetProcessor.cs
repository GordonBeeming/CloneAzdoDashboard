namespace CloneAzdoDashboard.WidgetProcessors
{
  public class IFrameWidgetProcessor : BaseWidgetProcessor
  {
    public override string ContributionId => "ms.vss-dashboards-web.Microsoft.VisualStudioOnline.Dashboards.IFrameWidget";

    public override void Run(DashboardInfo_Widget1 widget, AppConfig appConfig)
    {
      // nothing is required
    }
  }
}
