using Microsoft.Maui.Controls;
using VeterinariaApp.Models;
using VeterinariaApp.Services;
using Microsoft.Maui.Storage;

namespace VeterinariaApp.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly UsuarioService _usuarioService = new();

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnIngresarClicked(object sender, EventArgs e)
        {
            var usuario = usuarioEntry.Text;
            var contraseña = contraseñaEntry.Text;

            var usuarioValido = await _usuarioService.LoginAsync(usuario, contraseña);

            if (usuarioValido != null)
            {
                Preferences.Set("Rol", usuarioValido.Rol);

                // ✅ CAMBIO CLAVE: guardar como "NombreUsuario" para que el registro funcione
                Preferences.Set("NombreUsuario", usuarioValido.NombreUsuario);

                switch (usuarioValido.Rol)
                {
                    case "Administrador":
                        await Navigation.PushAsync(new MenuAdministradorPage());
                        break;
                    case "Veterinario":
                        await Navigation.PushAsync(new MenuVeterinarioPage());
                        break;
                    default:
                        await Navigation.PushAsync(new MenuUsuarioPage());
                        break;
                }
            }
            else
            {
                await DisplayAlert("Error", "Usuario o contraseña incorrectos.", "OK");
            }
        }

        private async void OnRegistrarseClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistroUsuarioPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            usuarioEntry.Text = string.Empty;
            contraseñaEntry.Text = string.Empty;
        }
    }
}
