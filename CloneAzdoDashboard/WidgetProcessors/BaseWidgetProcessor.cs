using System;

namespace CloneAzdoDashboard.WidgetProcessors
{
  public abstract class BaseWidgetProcessor
  {
    public abstract string ContributionId { get; }

    public abstract void Run(DashboardInfo_Widget1 widget, AppConfig appConfig);

    protected void WriteWarning(string message)
    {
      Console.ForegroundColor = ConsoleColor.DarkYellow;
      Console.WriteLine(message);
      Console.ForegroundColor = ConsoleColor.White;
    }
  }
}
