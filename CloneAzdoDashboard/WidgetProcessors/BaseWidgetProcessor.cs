namespace CloneAzdoDashboard.WidgetProcessors
{
  public abstract class BaseWidgetProcessor
  {
    public abstract string ContributionId { get; }

    public abstract void Run(DashboardInfo_Widget1 widget, AppConfig appConfig);
  }
}
