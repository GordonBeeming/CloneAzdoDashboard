using System.Linq;
using CloneAzdoDashboard.WidgetProcessors.WidgetySettings;
using Newtonsoft.Json;

namespace CloneAzdoDashboard.WidgetProcessors
{
  public class CodeScalarWidgetProcessor : BaseWidgetProcessor
  {
    public override string ContributionId => "ms.vss-dashboards-web.Microsoft.VisualStudioOnline.Dashboards.CodeScalarWidget";

    private static repositories _sourceRepositoryList = null;
    private static repositories _targetRepositoryList = null;

    public override void Run(DashboardInfo_Widget1 widget, AppConfig appConfig)
    {
      var settings = JsonConvert.DeserializeObject<CodeScalarWidgetSettings>(widget.settings);

      if (_sourceRepositoryList == null)
      {
        _sourceRepositoryList = TfsStatic.GetGitRepos(true);
      }
      if (_targetRepositoryList == null)
      {
        _targetRepositoryList = TfsStatic.GetGitRepos(false);
      }

      var sourceRepo = _sourceRepositoryList.value.FirstOrDefault(o => o.id == settings.repositoryId);
      var sourceRepoName = sourceRepo?.name;
      if (string.IsNullOrEmpty(sourceRepoName))
      {
        WriteWarning($"Skipped: Can't find a repository in the source project with an id of '{settings.repositoryId}'.");
        return;
      }
      var targetRepoId = _targetRepositoryList.value.FirstOrDefault(o => o.name == sourceRepoName)?.id;
      if (string.IsNullOrEmpty(targetRepoId))
      {
        WriteWarning($"Skipped: Can't find a repository in the target project with the name of '{sourceRepoName}'.");
        return;
      }

      settings.repositoryId = targetRepoId;

      widget.settings = JsonConvert.SerializeObject(settings);
    }
  }
}
