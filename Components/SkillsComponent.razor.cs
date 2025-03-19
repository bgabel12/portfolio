using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;


namespace BlazorWasmPortfolio.Components
{
  public partial class SkillsComponent
  {
    // defaults to true
    [Parameter]
    public bool DisplayShowCaseSkills { get; set; } = true;

    [Parameter]
    public bool DisplayAllSkills { get; set; } = true;

    [Parameter]
    public bool AllSkillsButton { get; set; } = true;
    
    private List<string> SkillCategories = [];
    private List<Skill> Skills = [];
    private List<string> ShowCaseCategories = [];
    private List<Skill> ShowCaseSkills = [];

    protected override async Task OnInitializedAsync()
    {
      Skills = await http.GetFromJsonAsync<List<Skill>>("content/skills.json") ?? [];
      SkillCategories = [.. Skills.DistinctBy(x => x.Category).Select(s => s.Category)];
      
      ShowCaseSkills = [.. Skills.Where(x => x.ShowCase)];
      ShowCaseCategories = [.. Skills.DistinctBy(x => x.Category).Select(s => s.Category)];

      _ = Skills.OrderBy(static i => i.Name);
      SkillCategories.Sort();

      _ = ShowCaseSkills.OrderBy(static i => i.Name);
      ShowCaseCategories.Sort();
    }

    public async Task<string> GetIconForSkill(string name)
    {
      return await jsRuntime.InvokeAsync<string>("getClassByName", name) ?? string.Empty;
    }
  }
}
