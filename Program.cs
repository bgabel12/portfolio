using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorWasmPortfolio;

public class Program
{
  public static async Task Main(string[] args)
  {
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");

    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
    builder.Services.AddBlazoredLocalStorage();
    await builder.Build().RunAsync();
  }
}

// See for deploying to github pages.
// https://github.com/jsakamoto/PublishSPAforGitHubPages.Build
// dotnet add package PublishSPAforGitHubPages.Build
// dotnet publish -c:Release -p:GHPages=true