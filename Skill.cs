namespace BlazorWasmPortfolio
{
  public class Skill
  {
    public string Category { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool ShowCase { get; set; } = false;
    public string ShowCaseIconCssClass { get; set; } = "fa-solid fa-check-to-slot";
  }
}
