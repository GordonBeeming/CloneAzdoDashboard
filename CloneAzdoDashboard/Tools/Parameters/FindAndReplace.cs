namespace CloneAzdoDashboard.Tools.Parameters
{
  public class FindAndReplace
  {
    public string Find { get; set; }
    public string Replace { get; set; }
    public bool TryRemoveSource { get; set; } = false;
    public bool TryRemoveTarget { get; set; } = false;
  }
}
