using CloneAzdoDashboard.WidgetProcessors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CloneAzdoDashboard
{
  class Program
  {
    static AppConfig config = null;

    static List<BaseWidgetProcessor> WidgetProcessors => new List<BaseWidgetProcessor> {
      new QueryScalarWidgetProcessor(),
      new WitViewWidgetProcessor(),
      new WitChartWidgetProcessor(),
      new MarkdownWidgetProcessor(),
    };

    static void Main(string[] args)
    {
      if (!LoadConfig(args))
      {
        return;
      }

      MigrateDashboard();

      DoneDone();
    }

    private static void MigrateDashboard()
    {
      if (TargetDashboardExists() && !config.DeleteTargetDashboardIfExists)
      {
        WriteLine($"Target dashboard '{config.TargetDashboardName}' in the team '{config.TargetTeamName}' already exists and DeleteTargetDashboardIfExists=false.", ConsoleColor.Red);
        return;
      }
      var output = string.Empty;
      var dashboards = TfsStatic.GetDashboards(true, config.SourceTeamName);
      var dashboard = dashboards.value.FirstOrDefault(o => o.name.Equals(config.SourceDashboardName));
      if (dashboard == null)
      {
        WriteLine($"Unable to find the dashboard '{config.SourceDashboardName}' in the team '{config.SourceTeamName}'.", ConsoleColor.Red);
        return;
      }
      WriteLine($"Source Dashboard: {dashboard.name} ({dashboard.id})");
      var dashboardInfo = TfsStatic.GetDashboard(true, config.SourceTeamName, dashboard.id);
      dashboardInfo.name = config.TargetDashboardName;

      WriteLine($"Widgets: {dashboardInfo.widgets.Length}");
      foreach (var widget in dashboardInfo.widgets)
      {
        WriteLine($"[{GetWidgetPositionDisplay(widget)}] {widget.name}");
        foreach (var processor in WidgetProcessors)
        {
          if (widget.contributionId.Equals(processor.ContributionId, StringComparison.InvariantCultureIgnoreCase))
          {
            WriteLine($"\tprocessing");
            processor.Run(widget, config);
            continue;
          }
        }
      }

      WriteLine();
      WriteLine();
      if (config.UpdateQueriesOnly)
      {
        WriteLine($"Skipping dashboard creation, UpdateQueriesOnly=true.");
      }
      else
      {
        DeleteDashboardIfExists();
        Write($"Creating dashboard '{config.TargetDashboardName}' in the team '{config.TargetTeamName}'...");
        TfsStatic.CreateDashboard(false, config.TargetTeamName, dashboardInfo);
        WriteLine("Done!", ConsoleColor.Green);
      }
    }

    private static void DeleteDashboardIfExists()
    {
      var dashboards = TfsStatic.GetDashboards(false, config.TargetTeamName);
      var dashboard = dashboards.value.FirstOrDefault(o => o.name.Equals(config.TargetDashboardName));
      if (dashboard != null)
      {
        WriteLine($"Deleting dashboard: {dashboard.name} ({dashboard.id})", ConsoleColor.DarkYellow);
        TfsStatic.DeleteDashboard(false, config.TargetTeamName, dashboard.id);
        return;
      }
    }

    private static bool TargetDashboardExists()
    {
      var dashboards = TfsStatic.GetDashboards(false, config.TargetTeamName);
      var dashboard = dashboards.value.FirstOrDefault(o => o.name.Equals(config.TargetDashboardName));
      return dashboard != null;
    }


    private static string GetWidgetPositionDisplay(DashboardInfo_Widget1 widget)
    {
      var display = widget.position.row.ToString();
      if (widget.size.rowSpan > 1)
      {
        display += $"-{widget.position.row + widget.size.rowSpan - 1 }";
      }
      display += $",{widget.position.column}";
      if (widget.size.columnSpan > 1)
      {
        display += $"-{widget.position.column + widget.size.columnSpan - 1 }";
      }
      return display;
    }

    #region fluff

    private static bool LoadConfig(string[] args)
    {
      var fileName = "config.json";
      if (args != null && args.Length > 0)
      {
        fileName = args[0].Trim('\"').Trim('\'');
      }
      SetConsoleThings();
      if (!File.Exists($".\\{fileName}"))
      {
        WriteLine($"{fileName} is missing!", ConsoleColor.Red);
        return false;
      }
      config = JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText($".\\{fileName}"));

      TfsStatic.SourceTeamProjectBaseUri = config.SourceTeamProjectBaseUri;
      TfsStatic.TargetTeamProjectBaseUri = config.TargetTeamProjectBaseUri;
      TfsStatic.SourcePatKey = config.SourcePatKey;
      TfsStatic.TargetPatKey = config.TargetPatKey;

      if (!GetPatToken())
      {
        return false;
      }

      return true;
    }

    private static bool GetPatToken()
    {
      Console.WriteLine("PAT keys can be generated in TFS, keep this safe. With this key we are able to impersonate you using the TFS API's.");
      Console.WriteLine("Steps to create: https://www.visualstudio.com/en-us/docs/setup-admin/team-services/use-personal-access-tokens-to-authenticate");
      Console.WriteLine("TFS Uri: https://{account}/{tpc}/_details/security/tokens");
      Console.WriteLine();
      if (string.IsNullOrEmpty(TfsStatic.SourcePatKey))
      {
        Console.WriteLine($"Source: {TfsStatic.SourceTeamProjectBaseUri}");
        Console.Write("Enter you Source PAT key: ");
        TfsStatic.SourcePatKey = Console.ReadLine();
        if ((TfsStatic.SourcePatKey?.Trim() ?? string.Empty).Length == 0)
        {
          Console.WriteLine();
          Console.WriteLine("Seems you didn't supply a key.");
          Console.ReadLine();
          return false;
        }
      }
      if (string.IsNullOrEmpty(TfsStatic.TargetPatKey))
      {
        Console.WriteLine($"Target: {TfsStatic.TargetTeamProjectBaseUri}");
        Console.Write("Enter you Target PAT key: ");
        TfsStatic.TargetPatKey = Console.ReadLine();
        if ((TfsStatic.TargetPatKey?.Trim() ?? string.Empty).Length == 0)
        {
          Console.WriteLine();
          Console.WriteLine("Seems you didn't supply a key.");
          Console.ReadLine();
          return false;
        }
      }
      Console.Clear();
      return true;
    }

    private static void SetConsoleThings()
    {
      Console.ForegroundColor = ConsoleColor.White;
      Console.BackgroundColor = ConsoleColor.Black;
      Console.Clear();
    }

    private static void Write(string message = "", ConsoleColor colour = ConsoleColor.White)
    {
      Console.ForegroundColor = colour;
      Console.Write(message);
      Console.ForegroundColor = ConsoleColor.White;
    }

    private static void WriteLine(string message = "", ConsoleColor colour = ConsoleColor.White)
    {
      Console.ForegroundColor = colour;
      Console.WriteLine(message);
      Console.ForegroundColor = ConsoleColor.White;
    }

    private static void DoneDone()
    {
      WriteLine();
      WriteLine();
      WriteLine();
      WriteLine("Done!");
      //Console.ReadLine();
    }

    #endregion
  }
}
