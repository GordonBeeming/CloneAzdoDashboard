using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace CloneAzdoDashboard
{
  public static class TfsStatic
  {
    #region core

    public const string BOOOOOOOM = "BOOOOOOOM!";
    public static string SourcePatKey = string.Empty;
    public static string TargetPatKey = string.Empty;
    public static string SourceTeamProjectBaseUri = string.Empty;
    public static string TargetTeamProjectBaseUri = string.Empty;

    private static string GetAuthorizationHeaderSource() => $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($":{SourcePatKey}"))}";
    private static string GetAuthorizationHeaderTarget() => $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($":{TargetPatKey}"))}";
    private static string GetAuthorizationHeader(bool source) => source ? GetAuthorizationHeaderSource() : GetAuthorizationHeaderTarget();

    private static T Get<T>(string uri, string authHeader)
    {
      using (var client = new WebClient())
      {
        client.Headers[HttpRequestHeader.Authorization] = authHeader;
        return TfsRestTry(uri, () =>
        {
          var responseString = client.DownloadString(uri);
          return JsonConvert.DeserializeObject<T>(responseString);
        });
      }
    }

    private static T GeneralPushData<T>(string uri, object data, string method, string contentType, string authHeader)
    {
      using (var client = new WebClient())
      {
        client.Headers[HttpRequestHeader.Authorization] = authHeader;
        client.Headers[HttpRequestHeader.ContentType] = contentType;
        var requestString = string.Empty;
        if (data != null)
        {
          requestString = JsonConvert.SerializeObject(data);
        }
        return TfsRestTry(uri, () =>
        {
          var responseString = client.UploadString(uri, method, requestString);
          return JsonConvert.DeserializeObject<T>(responseString);
        });
      }
    }

    private static void Post(string uri, object data, string authHeader)
    {
      Post<object>(uri, data, authHeader);
    }

    private static T Post<T>(string uri, object data, string authHeader)
    {
      return GeneralPushData<T>(uri, data, "POST", "application/json", authHeader);
    }

    private static void Patch(string uri, object data, string authHeader)
    {
      Patch<object>(uri, data, authHeader);
    }

    private static T Patch<T>(string uri, object data, string authHeader)
    {
      return GeneralPushData<T>(uri, data, "PATCH", "application/json", authHeader);
    }

    private static void Delete(string uri, object data, string authHeader)
    {
      Delete<object>(uri, data, authHeader);
    }

    private static T Delete<T>(string uri, object data, string authHeader)
    {
      return GeneralPushData<T>(uri, data, "DELETE", "application/json", authHeader);
    }

    private static void Put(string uri, object data, string authHeader)
    {
      GeneralPushData<object>(uri, data, "PUT", "application/json", authHeader);
    }

    private static void Patch2(string uri, object data, string authHeader)
    {
      Patch2<object>(uri, data, authHeader);
    }

    private static T Patch2<T>(string uri, object data, string authHeader)
    {
      return GeneralPushData<T>(uri, data, "PATCH", "application/json-patch+json", authHeader);
    }

    private static T TfsRestTry<T>(string uri, Func<T> f)
    {
      try
      {
        return f();
      }
      catch (WebException webEx) when (webEx.Status == WebExceptionStatus.ProtocolError && (((HttpWebResponse)webEx.Response).StatusCode == HttpStatusCode.BadRequest || ((HttpWebResponse)webEx.Response).StatusCode == HttpStatusCode.NotFound))
      {
        using (var sr = new StreamReader(webEx.Response.GetResponseStream()))
        {
          var responseString = sr.ReadToEnd();
          var exception = JsonConvert.DeserializeObject<RestCallException>(responseString);
          //throw new Exception($"{exception.message} | {uri}");
          if (exception != null)
          {
            throw exception;
          }
          throw webEx;
        }
      }
    }

    public static bool TryCreate(Action get, Action set)
    {
      return TryCreate(() => { get(); return string.Empty; }, set) != null;
    }

    public static string TryCreate(Func<string> get, Action set)
    {
      // this is bad =)
      try
      {
        Console.Write($"creating...");
        string result = get();
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine($"exists");
        Console.ForegroundColor = ConsoleColor.White;
        return result;
      }
      catch (Exception exThrow)
      {
        bool throwEx = false;
        try
        {
          set();
        }
        catch (Exception exx) when (exx.Message == TfsStatic.BOOOOOOOM)
        {
          throwEx = true;
        }
        if (throwEx)
        {
          throw exThrow;
        }
        try
        {
          string result = get();
          Console.ForegroundColor = ConsoleColor.DarkGreen;
          Console.WriteLine($"created");
          Console.ForegroundColor = ConsoleColor.White;
          return result;
        }
        catch (Exception ex)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine($"err: {ex.Message}");
          Console.ForegroundColor = ConsoleColor.White;
          return null;
        }
      }
    }

    #endregion

    public static repositories GetGitRepos(bool source)
    {
      return Get<repositories>(GetUrl(source, true, $"/_apis/git/repositories?api-version=1.0"), GetAuthorizationHeader(source));
    }

    public static CreateServiceEndpointResponse CreateServiceEndpoint(bool source, CreateServiceEndpointRequest request)
    {
      return Post<CreateServiceEndpointResponse>(GetUrl(source, false, $"/_apis/distributedtask/serviceendpoints?api-version=3.0-preview.1"), request, GetAuthorizationHeader(source));
    }

    public static CreateRepoResponse CreateRepo(bool source, CreateRepoRequest request)
    {
      return Post<CreateRepoResponse>(GetUrl(source, true, $"/_apis/git/repositories?api-version=1.0"), request, GetAuthorizationHeader(source));
    }

    public static CreateImportRequestResponse CreateImportRequest(bool source, string repoName, CreateImportRequestRequest request)
    {
      return Post<CreateImportRequestResponse>(GetUrl(source, false, $"/_apis/git/repositories/{repoName}/importRequests?api-version=5.0-preview.1"), request, GetAuthorizationHeader(source));
    }

    #region Dashboards

    public static DashboardsList GetDashboards(bool source, string teamName, bool projectDashboard)
    {
      var teamPart = projectDashboard ? string.Empty : $"/{teamName}";
      return Get<DashboardsList>(GetUrl(source, false, $"{teamPart}/_apis/dashboard/dashboards?api-version=6.0-preview.3"), GetAuthorizationHeader(source));
    }

    public static DashboardInfo GetDashboard(bool source, string teamName, bool projectDashboard, string dashboardId)
    {
      var teamPart = projectDashboard ? string.Empty : $"/{teamName}";
      return Get<DashboardInfo>(GetUrl(source, false, $"{teamPart}/_apis/dashboard/dashboards/{dashboardId}?api-version=6.0-preview.3"), GetAuthorizationHeader(source));
    }

    public static DashboardInfo CreateDashboard(bool source, string teamName, bool projectDashboard, DashboardInfo dashboardData)
    {
      var teamPart = projectDashboard ? string.Empty : $"/{teamName}";
      return Post<DashboardInfo>(GetUrl(source, false, $"{teamPart}/_apis/dashboard/dashboards?api-version=6.0-preview.3"), dashboardData, GetAuthorizationHeader(source));
    }

    public static void DeleteDashboard(bool source, string teamName, bool projectDashboard, string dashboardId)
    {
      var teamPart = projectDashboard ? string.Empty : $"/{teamName}";
      Delete(GetUrl(source, false, $"{teamPart}/_apis/dashboard/dashboards/{dashboardId}?api-version=6.0-preview.3"), null, GetAuthorizationHeader(source));
    }

    #endregion

    #region Work Item Queries

    public static WorkItemQueries GetWorkItemQueries(bool source)
    {
      return Get<WorkItemQueries>(GetUrl(source, false, $"/_apis/wit/queries?api-version=6.0"), GetAuthorizationHeader(source));
    }

    public static WorkItemQuery GetWorkItemQuery(bool source, string queryPath, QueryExpand queryExpand = QueryExpand.none, int depth = 2)
    {
      return Get<WorkItemQuery>(GetUrl(source, false, $"/_apis/wit/queries/{queryPath}?api-version=6.0&$expand={queryExpand}&$depth={depth}"), GetAuthorizationHeader(source));
    }

    public static WorkItemQuery CreateWorkItemQueryFolder(bool source, string queryPath, string folderName)
    {
      return Post<WorkItemQuery>(GetUrl(source, false, $"/_apis/wit/queries/{queryPath}?api-version=6.0"), new CreateWorkItemQueryFolderRequest { name = folderName }, GetAuthorizationHeader(source));
    }

    public static WorkItemQuery CreateWorkItemQuery(bool source, string queryPath, WorkItemQuery workItemQuery)
    {
      workItemQuery.id = Guid.Empty.ToString();
      return Post<WorkItemQuery>(GetUrl(source, false, $"/_apis/wit/queries/{queryPath}?api-version=6.0"), workItemQuery, GetAuthorizationHeader(source));
    }

    public static WorkItemQuery UpdateWorkItemQuery(bool source, WorkItemQuery workItemQuery)
    {
      workItemQuery.path += $"/{workItemQuery.name}";
      return Patch<WorkItemQuery>(GetUrl(source, false, $"/_apis/wit/queries/{workItemQuery.id}?api-version=6.0"), workItemQuery, GetAuthorizationHeader(source));
    }

    //public static void DeleteDashboard(bool source, string teamName, string dashboardId)
    //{
    //  Delete(GetUrl(source, false, $"/{teamName}/_apis/dashboard/dashboards/{dashboardId}?api-version=6.0-preview.3"), null, GetAuthorizationHeader(source));
    //}

    #endregion

    #region Builds

    public static BuildList GetBuilds(bool source)
    {
      return Get<BuildList>(GetUrl(source, false, $"/_apis/build/builds?api-version=6.0"), GetAuthorizationHeader(source));
    }

    #endregion

    #region Teams

    public static TeamList GetTeams(bool source, string projectName)
    {
      return Get<TeamList>(GetUrl(source, true, $"/_apis/projects/{projectName}/teams?api-version=6.0"), GetAuthorizationHeader(source));
    }

    #endregion

    public static string GetUrl(bool source, bool excludeProject, string uriRelativeToRoot)
    {
      var baseUri = source ? SourceTeamProjectBaseUri : TargetTeamProjectBaseUri;
      if (excludeProject)
      {
        baseUri = baseUri.Remove(baseUri.LastIndexOf('/'));
      }
      return $"{baseUri}{uriRelativeToRoot.Replace("//", "/")}";
    }

    public static string GetTeamProjectId(bool source)
    {
      var teamProjectName = GetTeamProjectName(source);
      var projects = Get<GetProjects>(GetUrl(source, true, $"/_apis/projects?api-version=2.0"), GetAuthorizationHeader(source));
      foreach (var item in projects.value)
      {
        if (item.name.Equals(teamProjectName, StringComparison.InvariantCultureIgnoreCase))
        {
          return item.id;
        }
      }
      return null;
    }

    public static string GetTeamProjectName(bool source)
    {
      var baseUri = source ? SourceTeamProjectBaseUri : TargetTeamProjectBaseUri;
      var teamProjectName = baseUri.Remove(0, baseUri.LastIndexOf('/') + 1);
      return teamProjectName;
    }
  }
}
