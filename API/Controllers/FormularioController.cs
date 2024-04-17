using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormularioController : ControllerBase
    {
        [HttpPost]
        
        public async Task<IActionResult> post([FromForm]ModelForm modelForm)
        {
            var requestobject= this.HttpContext.Request;
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage
                    (HttpMethod.Post, "https://challenges.cloudflare.com/turnstile/v0/siteverify");
                var pairs = new List<KeyValuePair<string, string>>
                {
                        new KeyValuePair<string, string>( "secret", "0x4AAAAAAAXPTqetFqR5nSkMaXfdENDtzU0" ),
                        new KeyValuePair<string, string>("response",modelForm.Token)
                    };
                var content = new FormUrlEncodedContent(pairs);

               

                
                httpRequestMessage.Content = content; //contenido;
                var response = await httpClient.SendAsync(httpRequestMessage);
                if (response.IsSuccessStatusCode)
                {
                    var respuesta= await response.Content.ReadAsStringAsync();
                    return Ok(respuesta);
                }
            }

            return Ok(modelForm);
        }
    }
}
