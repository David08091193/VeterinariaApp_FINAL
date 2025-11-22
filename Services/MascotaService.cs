using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VeterinariaApp.Models;
using System.Collections.Generic;

namespace VeterinariaApp.Services
{
    public class MascotaService
    {
        private readonly HttpClient _httpClient;

        public MascotaService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5104"); // Ajusta el puerto al de tu API
        }

        // Crear mascota
        public async Task<bool> CrearMascotaAsync(Mascota mascota)
        {
            var json = JsonSerializer.Serialize(mascota);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Mascota", content);
            return response.IsSuccessStatusCode;
        }

        // Obtener todas las mascotas
        public async Task<List<Mascota>> ObtenerMascotasAsync()
        {
            var response = await _httpClient.GetAsync("/api/Mascota");
            if (!response.IsSuccessStatusCode) return new List<Mascota>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Mascota>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Mascota>();
        }

        // Actualizar mascota
        public async Task<bool> ActualizarMascotaAsync(Mascota mascota)
        {
            var json = JsonSerializer.Serialize(mascota);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/Mascota/{mascota.Id}", content);
            return response.IsSuccessStatusCode;
        }

        // Eliminar mascota
        public async Task<bool> EliminarMascotaAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Mascota/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
