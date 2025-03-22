using Microsoft.JSInterop;
using Radzen;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BlazorWasmPortfolio.Pages
{
  public partial class Editor
  {
    private List<Project> Projects { get; set; } = [];
    private List<Skill> Skills { get; set; } = [];
    private string ProjectNamesValue { get; set; } = string.Empty;
    private string ProjectDescriptionValue { get; set; } = string.Empty;
    private string ProjectIconCssClassValue { get; set; } = string.Empty;
    private IEnumerable<string> IconCssClasses { get; set; } = [];
    private IEnumerable<string> ProjectNames { get; set; } = [];
    private string ProjectNameValue { get; set; } = string.Empty;
    private IEnumerable<ProjectTechnology> SelectedProjectTechnologies { get; set; } = [];
    private ProjectTechnology? TechnologyValue { get; set; } = new ();
    private string TechIconCssClassValue { get; set; } = string.Empty;
    private IEnumerable<string> TechIconCssClasses { get; set; } = [];
    private string TechNameValue { get; set; } = string.Empty;
    private string HtmlValue { get; set; } = string.Empty;
    private string ConfirmMsg { get; set; } = "Are you sure you want to proceed?";
    private string ConfirmAction { get; set; } = string.Empty;
    private object? ConfirmObject { get; set; } = new ();

    protected override async Task OnInitializedAsync()
    {
      Projects = await http.GetFromJsonAsync<List<Project>>("content/projects.json") ?? [];
      ProjectNames = Projects.DistinctBy(x => x.Name).Select(s => s.Name);

      Skills = await http.GetFromJsonAsync<List<Skill>>("content/skills.json") ?? [];
      IconCssClasses = await jsRuntime.InvokeAsync<IEnumerable<string>>("getIconsClasses", null);
      
      // not sure if this is needed, might be able to re-use IconCssClasses
      TechIconCssClasses = IconCssClasses;

      await base.OnInitializedAsync();
    }

    private void CreateProject()
    {
      // TODO: Check if HtmlValue is empty first or has changed since the last create/load, if not warn the user the content in the editor will be lost.


      if (Projects.Any(x => x.Name == ProjectNameValue))
      {
        // TODO: Alert the user the project name already exists.
        return;
      }

      ProjectNamesValue = string.Empty;
      HtmlValue = string.Empty;
      Projects.Add(new Project
      {
        Name = ProjectNameValue,
        Description = ProjectDescriptionValue,
        IconCssClass = ProjectIconCssClassValue,
        Technologies = [],
      });
    }

    private async Task OpenTechAsync()
    {
      await jsRuntime.InvokeVoidAsync("showModal", "techModal");
    }

    private async Task OnConfirmAsync()
    {
      if (ConfirmObject != null)
      {
        if (ConfirmAction == "Delete")
        {
          if (ConfirmObject.GetType() == typeof(ProjectTechnology))
          {
            var projname = !string.IsNullOrEmpty(ProjectNameValue) ? ProjectNameValue : ProjectNamesValue;
            Projects.FirstOrDefault(x => x.Name == projname)?.Technologies.Remove((ProjectTechnology)ConfirmObject);
            TechnologyValue = null;
          }
        }
        else if (ConfirmAction == "Loss") // Confirm loss of edits..
        {
          // TODO:
        }
      }

      await jsRuntime.InvokeVoidAsync("hideModal", "confirmModal");
    }

    private async Task ConfirmDeleteTechAsync()
    {
      ConfirmObject = TechnologyValue;
      ConfirmAction = "Delete";
      await jsRuntime.InvokeVoidAsync("showModal", "confirmModal");
    }

    private void AddTech()
    {
      if (string.IsNullOrWhiteSpace(TechNameValue)) return;

      var projname = !string.IsNullOrEmpty(ProjectNameValue) ? ProjectNameValue : ProjectNamesValue;
      var techlist = Projects.FirstOrDefault(x => x.Name == projname)?.Technologies;
      if (techlist != null && techlist.FirstOrDefault(t => t.Name.Equals(TechNameValue.ToLower(), StringComparison.CurrentCultureIgnoreCase)) == null)
      {
        techlist.Add(new ProjectTechnology 
        {
          Name = TechNameValue,
          IconCssClass = TechIconCssClassValue
        });
        TechNameValue = string.Empty;
        TechIconCssClassValue = string.Empty;
      }
    }

    private async Task DownloadFromStreamAsync()
    {
      var fileName = string.IsNullOrEmpty(ProjectNamesValue) 
        ? string.IsNullOrWhiteSpace(ProjectNameValue) 
          ? ProjectNamesValue 
          : ProjectNameValue
        : ProjectNamesValue;

      // Set the Html to be downloaded as .txt
      await DownloadService.DownloadFileAsync($"{fileName}.txt", new MemoryStream(Encoding.UTF8.GetBytes(HtmlValue)));

      // Projects list to json
      string json = JsonSerializer.Serialize(Projects);
      await DownloadService.DownloadFileAsync("projects.json", new MemoryStream(Encoding.UTF8.GetBytes(json)));

      // TODO: Let the user know they must add these to the repo
      // wwwroot/content/projects/projects_name.txt
      // wwwroot/content/projects.json
    }

    private async Task LoadProject()
    {
      // load tech
      var projname = !string.IsNullOrEmpty(ProjectNameValue) ? ProjectNameValue : ProjectNamesValue;
      if (string.IsNullOrEmpty(projname)) return;

      var proj = Projects.FirstOrDefault(x => x.Name == projname);
      SelectedProjectTechnologies = proj?.Technologies ?? [];

      // load file
      HtmlValue = await ReadFile($"{ProjectNamesValue}.txt");
      ProjectNameValue = string.Empty;
      ProjectDescriptionValue = string.Empty;
      ProjectIconCssClassValue = string.Empty;
    }

    private async Task<string> ReadFile(string fileName)
    {
      var filePath = !NavManager.Uri.ToString().Contains("localhost", StringComparison.InvariantCultureIgnoreCase) ? "/portfolio/content/projects/" + fileName : "/content/projects/" + fileName;
      HttpResponseMessage response = await http.GetAsync(filePath);
      HttpContent content = response.Content;

      return await content.ReadAsStringAsync();
    }

    private void OnEditorPaste(HtmlEditorPasteEventArgs args)
    {
      //Console.WriteLine($"Paste: {args.Html}");
    }

    private void OnEditorChange(string html)
    {
      //Console.WriteLine($"Change: {html}");
    }

    private void OnEditorInput(string html)
    {
      //Console.WriteLine($"Input: {html}");
    }

    private void OnEditorExecute(HtmlEditorExecuteEventArgs args)
    {
      // A callback that will be invoked when the user executes a command of the editor (e.g. by clicking one of the tools).
      //Console.WriteLine($"Execute: {args.CommandName}");
    }
  }
}
