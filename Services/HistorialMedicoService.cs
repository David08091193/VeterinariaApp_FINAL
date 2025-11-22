using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VeterinariaApp.Models;
using System.Collections.Generic;

namespace VeterinariaApp.Services
{
    public class HistorialMedicoService
    {
        private readonly HttpClient _httpClient;

        public HistorialMedicoService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5104"); // Ajusta el puerto al de tu API
        }

        public async Task<bool> CrearHistorialAsync(HistorialMedico historial)
        {
            var json = JsonSerializer.Serialize(historial);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/HistorialMedico", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<HistorialMedico>> ObtenerHistorialesAsync()
        {
            var response = await _httpClient.GetAsync("/api/HistorialMedico");
            if (!response.IsSuccessStatusCode) return new List<HistorialMedico>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<HistorialMedico>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<HistorialMedico>();
        }

        public async Task<List<HistorialMedico>> ObtenerHistorialPorMascotaAsync(string nombreMascota)
        {
            var response = await _httpClient.GetAsync($"/api/HistorialMedico/por-mascota/{nombreMascota}");
            if (!response.IsSuccessStatusCode) return new List<HistorialMedico>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<HistorialMedico>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<HistorialMedico>();
        }

        public async Task<bool> EliminarHistorialAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/HistorialMedico/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
