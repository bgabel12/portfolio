// Home.razor.cs
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace BlazorWasmPortfolio.Pages
{
  public partial class Home
  {
    public string ImageTitle = string.Empty;
    public string ImageFileName = "player_manager.png";
    
    //protected override async Task OnInitializedAsync()
    //{
      
    //}

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
