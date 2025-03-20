namespace BlazorWasmPortfolio
{
  public class Project
  {
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconCssClass { get; set; } = string.Empty;
    public List<ProjectTechnology> Technologies { get; set; } = [];
  }

  public class ProjectTechnology
  {
    public string Name { get; set; } = string.Empty;
    public string IconCssClass { get; set; } = string.Empty;
  }
}
