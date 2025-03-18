// Home.razor.cs
using Microsoft.JSInterop;

namespace BlazorWasmPortfolio.Pages
{
  public partial class Home
  {
    public string ImageTitle = string.Empty;
    public string ImageFileName = string.Empty;
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
