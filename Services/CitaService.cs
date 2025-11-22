using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VeterinariaApp.Models;
using System.Collections.Generic;

namespace VeterinariaApp.Services
{
    public class CitaService
    {
        private readonly HttpClient _httpClient;

        public CitaService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5104"); // Ajusta el puerto al de tu API
        }

        // Crear cita
        public async Task<bool> CrearCitaAsync(Cita cita)
        {
            var json = JsonSerializer.Serialize(cita);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Cita", content);
            return response.IsSuccessStatusCode;
        }

        // Obtener todas las citas
        public async Task<List<Cita>> ObtenerCitasAsync()
        {
            var response = await _httpClient.GetAsync("/api/Cita");
            if (!response.IsSuccessStatusCode) return new List<Cita>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Cita>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Cita>();
        }

        // Obtener citas por usuario
        public async Task<List<Cita>> ObtenerCitasPorUsuarioAsync(string usuario)
        {
            var response = await _httpClient.GetAsync($"/api/Cita/por-usuario/{usuario}");
            if (!response.IsSuccessStatusCode) return new List<Cita>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Cita>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Cita>();
        }

        // Eliminar cita
        public async Task<bool> EliminarCitaAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Cita/{id}");
            return response.IsSuccessStatusCode;
        }


    }
}
