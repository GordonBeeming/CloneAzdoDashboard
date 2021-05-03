using System.Collections.Generic;

namespace CloneAzdoDashboard.Tools.Parameters
{
  public class QueryReplacementParameters
  {
    public string PathFind { get; set; }
    public string PathReplace { get; set; }

    public List<FindAndReplace> QueryFindAndReplace { get; set; } = new List<FindAndReplace>();
  }
}
