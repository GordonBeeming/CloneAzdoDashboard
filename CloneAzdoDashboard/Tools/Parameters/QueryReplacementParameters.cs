namespace CloneAzdoDashboard.Tools.Parameters
{
  public class QueryReplacementParameters
  {
    public string PathFind { get; set; }
    public string PathReplace { get; set; }

    public FindAndReplace[] QueryFindAndReplace { get; set; } = new FindAndReplace[0];
  }
}
