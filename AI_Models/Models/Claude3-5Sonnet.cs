using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AI_Models.Models
{
    internal class Claude3_5Sonnet
    {
        private readonly string apiKey;
        private readonly string apiEndpoint;
        private readonly string model;
        private readonly HttpClient client;

        public Claude3_5Sonnet(string apiKey, string apiEndpoint, string model)
        {
            this.apiKey = apiKey;
            this.apiEndpoint = apiEndpoint;
            this.model = model;
            this.client = new HttpClient();
            this.client.DefaultRequestHeaders.Add("x-api-key", this.apiKey);
            this.client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> EnviarMensaje(string mensaje)
        {
            var requestBody = new
            {
                model = this.model,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = mensaje
                    }
                },
                max_tokens = 150
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(this.apiEndpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error en la solicitud: {response.ReasonPhrase}. Detalles: {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic responseObject = JsonConvert.DeserializeObject(responseContent);

            // Acceder al texto de la respuesta
            return responseObject.content[0].text.ToString();
        }
    }
}
