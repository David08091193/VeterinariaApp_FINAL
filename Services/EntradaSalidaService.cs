using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VeterinariaApp.Models;
using System.Collections.Generic;

namespace VeterinariaApp.Services
{
    public class EntradaSalidaService
    {
        private readonly HttpClient _httpClient;

        public EntradaSalidaService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5104"); // Ajusta el puerto al de tu API
        }

        public async Task<bool> CrearRegistroAsync(EntradaSalida registro)
        {
            var json = JsonSerializer.Serialize(registro);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/EntradaSalida", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<EntradaSalida>> ObtenerRegistrosAsync()
        {
            var response = await _httpClient.GetAsync("/api/EntradaSalida");
            if (!response.IsSuccessStatusCode) return new List<EntradaSalida>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<EntradaSalida>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<EntradaSalida>();
        }

        public async Task<bool> EliminarRegistroAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/EntradaSalida/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
