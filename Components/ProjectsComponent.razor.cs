using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace BlazorWasmPortfolio.Components
{
  public partial class ProjectsComponent
  {
    private List<Project> Projects = [];
    public string ImageTitle = string.Empty;
    public string ImageFileName = "player_manager.png";
    
    protected override async Task OnInitializedAsync()
    {
      Projects = await http.GetFromJsonAsync<List<Project>>("content/projects.json") ?? [];
    }

    protected override async void OnAfterRender(bool firstRender)
    {
      // execute conditionally for loading data, otherwise this will load
      // every time the page refreshes
      //if (firstRender) { }

      var cnt = 0;
      foreach (var project in Projects)
      {
        var id = "projectAccordion" + cnt++ + "body";
        var body = await ReadFile(project.Name + ".txt");
        await jsRuntime.InvokeVoidAsync("setInnerHtml", id, body);
      }
    }

    private async Task<string> ReadFile(string fileName)
    {
      var filePath = !NavManager.Uri.ToString().Contains("localhost", StringComparison.InvariantCultureIgnoreCase) ? "/portfolio/content/projects/" + fileName : "/content/projects/" + fileName;
      HttpResponseMessage response = await http.GetAsync(filePath);
      HttpContent content = response.Content;

      return await content.ReadAsStringAsync();
    }

    /// <summary>
    /// Onclick event for images
    /// Note: The onclick events is used in the injected HTML from the content/projects/*.txt files
    /// </summary>
    async Task OnImageClick(string title, string fileName)
    {
      ImageTitle = title;
      ImageFileName = fileName;
      await jsRuntime.InvokeVoidAsync("showModal", "imageModal");
    }
  }
}
