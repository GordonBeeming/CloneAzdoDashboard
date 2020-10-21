﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using CloneAzdoDashboard.Tools.Parameters;

namespace CloneAzdoDashboard.Tools
{
  public static class QueryTools
  {
    public static WorkItemQuery CopyQuery(CopyQueryParameters parameters)
    {
      var sourceQuery = TfsStatic.GetWorkItemQuery(true, parameters.QueryId, QueryExpand.minimal, 0);

      var targetQueryInfo = GetTargetQueryFolderId(sourceQuery, parameters.QueryReplacements);

      if (TfsStatic.SourceTeamProjectBaseUri.Equals(TfsStatic.TargetTeamProjectBaseUri, StringComparison.InvariantCultureIgnoreCase) &&
          sourceQuery.path.Equals($"{targetQueryInfo.FolderPath}/{targetQueryInfo.QueryName}", StringComparison.InvariantCultureIgnoreCase))
      {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Skipped: Target query path matches source query ({targetQueryInfo.FolderPath}/{targetQueryInfo.QueryName})");
        Console.ForegroundColor = ConsoleColor.White;
        return sourceQuery;
      }

      sourceQuery.name = targetQueryInfo.QueryName;
      sourceQuery.path = targetQueryInfo.FolderPath;
      FindAndReplaceInWiql(parameters, sourceQuery);
      RemoveTeamAreaId(sourceQuery);

      WorkItemQuery result;
      int tryCount = 0;
      while (true)
      {
        try
        {
          result = TryWriteQuery(sourceQuery, targetQueryInfo);
          break;
        }
        catch
        {
          tryCount++;
          if (tryCount >= 5)
          {
            throw;
          }
        }
        Thread.Sleep(2500);
      }
      return result;
    }

    private static WorkItemQuery TryWriteQuery(WorkItemQuery sourceQuery, TargetQueryInfo targetQueryInfo)
    {
      WorkItemQuery targetQuery;
      var queryExistsAlready = false;
      try
      {
        var targetFolder = TfsStatic.GetWorkItemQuery(true, targetQueryInfo.FolderId, QueryExpand.minimal, 1);
        if (targetFolder.hasChildren)
        {
          targetQuery = targetFolder.children.FirstOrDefault(o => o.name.Equals(targetQueryInfo.QueryName, StringComparison.InvariantCultureIgnoreCase));
          if (targetQuery != null)
          {
            queryExistsAlready = true;
            sourceQuery.id = targetQuery.id;
            targetQuery = TfsStatic.UpdateWorkItemQuery(false, sourceQuery);
          }
          else
          {
            targetQuery = TfsStatic.CreateWorkItemQuery(false, targetQueryInfo.FolderId, sourceQuery);
          }
        }
        else
        {
          targetQuery = TfsStatic.CreateWorkItemQuery(false, targetQueryInfo.FolderId, sourceQuery);
        }
      }
      catch
      {
        if (queryExistsAlready)
        {
          throw;
        }
        targetQuery = TfsStatic.CreateWorkItemQuery(false, targetQueryInfo.FolderId, sourceQuery);
      }

      return targetQuery;
    }

    private static void RemoveTeamAreaId(WorkItemQuery sourceQuery)
    {
      var pattern = @"\s<id:\w+-\w+-\w+-\w+-\w+>";
      var regex = new Regex(pattern);
      var matches = regex.Matches(sourceQuery.wiql);
      for (int i = matches.Count - 1; i >= 0; i--)
      {
        Match match = matches[i];
        if (match.Success)
        {
          sourceQuery.wiql = sourceQuery.wiql.Remove(match.Index, match.Length);
        }
      }
    }

    private static void FindAndReplaceInWiql(CopyQueryParameters parameters, WorkItemQuery sourceQuery)
    {
      if (parameters.QueryReplacements.QueryFindAndReplace != null && parameters.QueryReplacements.QueryFindAndReplace.Length > 0)
      {
        foreach (var item in parameters.QueryReplacements.QueryFindAndReplace)
        {
          if (!string.IsNullOrEmpty(item.Find) && !string.IsNullOrEmpty(item.Replace) &&
              !item.Find.Equals(item.Replace, StringComparison.InvariantCultureIgnoreCase))
          {
            var findIndex = sourceQuery.wiql.IndexOf(item.Find, StringComparison.InvariantCultureIgnoreCase);
            while (findIndex > -1)
            {
              sourceQuery.wiql = sourceQuery.wiql.Remove(findIndex, item.Find.Length);
              sourceQuery.wiql = sourceQuery.wiql.Insert(findIndex, item.Replace);
              findIndex = sourceQuery.wiql.IndexOf(item.Find, findIndex + item.Replace.Length, StringComparison.InvariantCultureIgnoreCase);
            }
          }
        }
      }
    }

    private static TargetQueryInfo GetTargetQueryFolderId(WorkItemQuery sourceQuery, QueryReplacementParameters replacementParameters)
    {
      TargetQueryInfo targetQueryFolder = new TargetQueryInfo();
      targetQueryFolder.FolderPath = sourceQuery.path;
      if (!string.IsNullOrEmpty(replacementParameters.PathFind) && !string.IsNullOrEmpty(replacementParameters.PathReplace) &&
          !replacementParameters.PathFind.Equals(replacementParameters.PathReplace, StringComparison.InvariantCultureIgnoreCase))
      {
        var findIndex = targetQueryFolder.FolderPath.IndexOf(replacementParameters.PathFind, StringComparison.InvariantCultureIgnoreCase);
        while (findIndex > -1)
        {
          targetQueryFolder.FolderPath = targetQueryFolder.FolderPath.Remove(findIndex, replacementParameters.PathFind.Length);
          targetQueryFolder.FolderPath = targetQueryFolder.FolderPath.Insert(findIndex, replacementParameters.PathReplace);
          findIndex = targetQueryFolder.FolderPath.IndexOf(replacementParameters.PathFind, findIndex + replacementParameters.PathReplace.Length, StringComparison.InvariantCultureIgnoreCase);
        }
      }
      targetQueryFolder.QueryName = targetQueryFolder.FolderPath.Remove(0, targetQueryFolder.FolderPath.LastIndexOf('/') + 1);
      targetQueryFolder.FolderPath = targetQueryFolder.FolderPath.Remove(targetQueryFolder.FolderPath.LastIndexOf('/'));
      try
      {
        targetQueryFolder.FolderId = TfsStatic.GetWorkItemQuery(false, targetQueryFolder.FolderPath, QueryExpand.minimal, 0).id;
      }
      catch
      {
        string pathLeft = targetQueryFolder.FolderPath;
        do
        {
          pathLeft = pathLeft.Remove(pathLeft.LastIndexOf('/'));
          try
          {
            targetQueryFolder.FolderId = TfsStatic.GetWorkItemQuery(false, pathLeft, QueryExpand.minimal, 0).id;
            break;
          }
          catch { }
        }
        while (string.IsNullOrEmpty(targetQueryFolder.FolderId));

        do
        {
          var nextFolder = targetQueryFolder.FolderPath.Remove(0, pathLeft.Length + 1);
          if (nextFolder.IndexOf('/') > -1)
          {
            nextFolder = nextFolder.Remove(nextFolder.IndexOf('/'));
          }
          targetQueryFolder.FolderId = TfsStatic.CreateWorkItemQueryFolder(false, pathLeft, nextFolder).id;
          pathLeft = $"{pathLeft}/{nextFolder}";
        }
        while (targetQueryFolder.FolderPath.Length != pathLeft.Length);
      }

      return targetQueryFolder;
    }

  }
}
