using CloneAzdoDashboard.WidgetProcessors.WidgetySettings;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace CloneAzdoDashboard.WidgetProcessors
{
  public class BuildHistogramWidgetProcessor : BaseWidgetProcessor
  {
    public override string ContributionId => "ms.vss-dashboards-web.Microsoft.VisualStudioOnline.Dashboards.BuildHistogramWidget";

    private static BuildList targetBuildList = null;

    public override void Run(DashboardInfo_Widget1 widget, AppConfig appConfig)
    {
      var settings = JsonConvert.DeserializeObject<BuildHistogramWidgetSettings>(widget.settings);

      if (targetBuildList == null)
      {
        targetBuildList = TfsStatic.GetBuilds(false);
      }

      var targetBuildName = settings.buildDefinition.name;
      if (appConfig.Builds.Mapping.Keys.Any(o => o.Equals(targetBuildName, StringComparison.InvariantCultureIgnoreCase)))
      {
        targetBuildName = appConfig.Builds.Mapping[appConfig.Builds.Mapping.Keys.First(o => o.Equals(targetBuildName, StringComparison.InvariantCultureIgnoreCase))];
      }
      var targetBuild = targetBuildList.value.FirstOrDefault(o => o.definition.name.Equals(targetBuildName, StringComparison.InvariantCultureIgnoreCase));

      if (targetBuild == null)
      {
        WriteWarning($"Skipped: Build named '{targetBuildName}' does not exist in target project");
        return;
      }
      if (widget.name.Equals(settings.buildDefinition.name, StringComparison.InvariantCultureIgnoreCase))
      {
        widget.name = targetBuildName;
      }
      settings.buildDefinition.id = targetBuild.definition.id;
      settings.buildDefinition.name = targetBuild.definition.name;
      settings.buildDefinition.projectId = targetBuild.definition.project.id;
      settings.buildDefinition.uri = targetBuild.definition.uri;

      widget.settings = JsonConvert.SerializeObject(settings);
    }
  }
}
