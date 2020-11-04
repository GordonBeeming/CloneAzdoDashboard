namespace CloneAzdoDashboard.WidgetProcessors
{
  public class NewWorkItemWidgetProcessor : BaseWidgetProcessor
  {
    public override string ContributionId => "ms.vss-dashboards-web.Microsoft.VisualStudioOnline.Dashboards.NewWorkItemWidget";

    public override void Run(DashboardInfo_Widget1 widget, AppConfig appConfig)
    {
      // nothing is required
    }
  }
}
