using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorGithubEnv.Layout
{
    public partial class MainLayout
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        public NavigationManager NavigationManager { get; set; }
        public ModelForm Model { get; set; }
        public static string tokenValidate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //NavigationManager.NavigateTo("");
            Model= new ModelForm();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
            {
                await JSRuntime.InvokeVoidAsync("loadcloud");
                tokenValidate = await JSRuntime.InvokeAsync<string>("GetData");
                Console.WriteLine($"token que se envia {tokenValidate}");
            }
        }
        private async Task OnFormValid()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage
                    (HttpMethod.Post, "https://localhost:7129/api/formulario");
                var pairs = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>( "Nombre", $"{Model.Nombre}" ),
                        new KeyValuePair<string, string>("Token",tokenValidate)
                    };
                var content = new FormUrlEncodedContent(pairs);
                
                string data=JsonSerializer.Serialize(Model);
                
                StringContent contenido = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
                httpRequestMessage.Content = content; //contenido;
                var response= await httpClient.SendAsync(httpRequestMessage);
                if(response.IsSuccessStatusCode)
                {
                    Console.WriteLine("llego el api");
                }
            }
        }
        [JSInvokable]
        public static void GetDataFromJs(string data)
        {
            tokenValidate = data;
            Console.WriteLine("llego desde js con " + data);
            Console.WriteLine("ahora llego desde js con " + tokenValidate);
        }
    }
}