using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorGithubEnv.Layout
{
    public partial class MainLayout
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        protected override async Task OnInitializedAsync()
        {
           
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
            {
                await JSRuntime.InvokeVoidAsync("loadcloud");
            }
        }
    }
}