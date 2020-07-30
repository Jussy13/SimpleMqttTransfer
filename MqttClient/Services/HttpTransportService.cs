using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Database.Models;

namespace MqttClient.Services
{
    public class HttpTransportService
    {
        private readonly HttpClient _httpClient;

        public HttpTransportService(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task SendMessageAsync(Message message)
        {
            var messageJson = new StringContent(
                JsonSerializer.Serialize(message),
                Encoding.UTF8,
                "application/json"
            );

            using var httpResponse =
                await _httpClient.PostAsync(_httpClient.BaseAddress, messageJson);

            httpResponse.EnsureSuccessStatusCode();
        }
    }
}
