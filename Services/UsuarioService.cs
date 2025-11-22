using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VeterinariaApp.Models;

namespace VeterinariaApp.Services
{
    public class UsuarioService
    {
        private readonly HttpClient _httpClient;

        public UsuarioService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5104"); // Ajusta el puerto al de tu API
        }


        public async Task<Usuario?> LoginAsync(string nombreUsuario, string contraseña)
        {
            var loginRequest = new { NombreUsuario = nombreUsuario, Contraseña = contraseña };
            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/Usuario/login", content);
            if (!response.IsSuccessStatusCode) return null;

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Usuario>(responseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<bool> RegistrarAsync(Usuario usuario)
        {
            var json = JsonSerializer.Serialize(usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/Usuario/registro", content);
            return response.IsSuccessStatusCode;
        }
    }
}
