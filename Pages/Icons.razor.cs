// Icons.razor.cs
using Microsoft.JSInterop;

namespace BlazorWasmPortfolio.Pages
{
  public partial class Icons
  {
    public async Task GetIcons(string lib)
    {
      await jsRuntime.InvokeVoidAsync("displayIcons", lib);
    }
  }
}
