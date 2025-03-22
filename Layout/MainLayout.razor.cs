// MainLayout.razor.cs
using Microsoft.JSInterop;

namespace BlazorWasmPortfolio.Layout
{
  public partial class MainLayout
  {
    public bool IsLocalHost;
    public string ThemeToggleText = "🌙 Dark Mode";
    private bool DarkMode;

    protected override async Task OnInitializedAsync()
    {
      IsLocalHost = NavManager.Uri.ToString().Contains("localhost", StringComparison.InvariantCultureIgnoreCase);
      var theme = await localStorage.GetItemAsync<string>("theme") ?? "light";
      await SetTheme(theme);
    }

    async Task OnThemeClick()
    {
      await SetTheme(DarkMode ? "light" : "dark");
    }

    async Task OnBackToTopClick()
    {
      await jsRuntime.InvokeVoidAsync("backToTop");
    }

    private async Task SetTheme(string theme)
    {
      await jsRuntime.InvokeVoidAsync("setTheme", theme);
      await localStorage.SetItemAsync("theme", theme);
      DarkMode = theme == "dark";
      ThemeToggleText = DarkMode ? "🌞 Light Mode" : "🌙 Dark Mode";
    }
  }
}
