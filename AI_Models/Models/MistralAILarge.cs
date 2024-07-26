using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AI_Models.Models
{
    internal class MistralAILarge
    {
        private readonly string apiKey;
        private readonly string apiEndpoint;
        private readonly string model;
        private readonly HttpClient client;

        public MistralAILarge(string apiKey, string apiEndpoint, string model)
        {
            this.apiKey = apiKey;
            this.apiEndpoint = apiEndpoint;
            this.model = model;
            this.client = new HttpClient();
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.apiKey);
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
        }
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(this.apiEndpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error en la solicitud: {response.ReasonPhrase}. Detalles: {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic responseObject = JsonConvert.DeserializeObject(responseContent);

            // Validar y acceder al texto de la respuesta
            if (responseObject?.choices != null && responseObject.choices.Count > 0 && responseObject.choices[0].message != null)
            {
                return responseObject.choices[0].message.content.ToString();
            }
            else
            {
                throw new Exception($"La respuesta de la API no contiene el contenido esperado. Respuesta completa: {responseContent}");
            }
        }
    }
}
