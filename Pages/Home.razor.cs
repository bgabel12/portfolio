// Home.razor.cs
using Microsoft.JSInterop;

namespace BlazorWasmPortfolio.Pages
{
  public partial class Home
  {
    /// <summary>
    /// Onclick event for images
    /// </summary>
    /// <param name="modal">The modal to display</param>
    async Task OnImageClick(string modal)
    {
      await jsRuntime.InvokeVoidAsync("showModal", modal);
    }
  }
}
