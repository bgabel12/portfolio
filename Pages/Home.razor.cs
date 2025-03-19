// Home.razor.cs
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace BlazorWasmPortfolio.Pages
{
  public partial class Home
  {
    private List<string> SkillCategories = [];
    private List<Skill> Skills = [];
    public string ImageTitle = string.Empty;
    public string ImageFileName = "player_manager.png";
    
    protected override async Task OnInitializedAsync()
    {
      Skills = await http.GetFromJsonAsync<List<Skill>>("content/skills.json") ?? [];
      Skills.OrderBy(i => i.Name);
      SkillCategories = Skills.DistinctBy(x => x.Category).Select(s => s.Category).ToList();
    }

    /// <summary>
    /// Onclick event for images
    /// </summary>
    /// <param name="modal">The modal to display</param>
    async Task OnImageClick(string title, string fileName)
    {
      ImageTitle = title;
      ImageFileName = fileName;
      await jsRuntime.InvokeVoidAsync("showImageModal");
    }
  }
}
