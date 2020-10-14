using System;

namespace CloneAzdoDashboard.WidgetProcessors
{
  public class MarkdownWidgetProcessor : BaseWidgetProcessor
  {
    public override string ContributionId => "ms.vss-dashboards-web.Microsoft.VisualStudioOnline.Dashboards.MarkdownWidget";

    public override void Run(DashboardInfo_Widget1 widget, AppConfig appConfig)
    {
      var findIndex = widget.settings.IndexOf(appConfig.SourceTeamName, StringComparison.InvariantCultureIgnoreCase);
      if (findIndex > -1)
      {
        widget.settings = widget.settings.Remove(findIndex, appConfig.SourceTeamName.Length);
        widget.settings = widget.settings.Insert(findIndex, appConfig.TargetTeamName);
      }

      if (appConfig.MarkdownFindAndReplace != null)
      {
        foreach (var item in appConfig.MarkdownFindAndReplace)
        {
          if (!string.IsNullOrEmpty(item.Find) && !string.IsNullOrEmpty(item.Replace))
          {
            findIndex = widget.settings.IndexOf(item.Find, StringComparison.InvariantCultureIgnoreCase);
            while (findIndex > -1)
            {
              widget.settings = widget.settings.Remove(findIndex, item.Find.Length);
              widget.settings = widget.settings.Insert(findIndex, item.Replace);
              findIndex = widget.settings.IndexOf(item.Find, StringComparison.InvariantCultureIgnoreCase);
            }
          }
        }
      }
    }
  }
}
