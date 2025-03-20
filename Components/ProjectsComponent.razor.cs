using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace BlazorWasmPortfolio.Components
{
  public partial class ProjectsComponent
  {
    private List<Project> Projects = [];
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
      HttpResponseMessage response = await http.GetAsync("/content/projects/" + fileName);
      HttpContent content = response.Content;

      return await content.ReadAsStringAsync();
    }
  }
}
