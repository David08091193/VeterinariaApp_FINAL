using Microsoft.Maui.Controls;
using VeterinariaApp.Models;

namespace VeterinariaApp.Views
{
    public partial class RegistroUsuarioPage : ContentPage
    {
        public RegistroUsuarioPage()
        {
            InitializeComponent();
        }

        private async void OnCrearCuentaClicked(object sender, EventArgs e)
        {
            var usuario = usuarioEntry.Text;
            var contraseña = contraseñaEntry.Text;
            var confirmar = confirmarEntry.Text;

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contraseña))
            {
                await DisplayAlert("Campos vacíos", "Por favor ingresa usuario y contraseña.", "OK");
                return;
            }

            if (contraseña != confirmar)
            {
                await DisplayAlert("Error", "Las contraseñas no coinciden.", "OK");
                return;
            }

            var nuevoUsuario = new Usuario
            {
                NombreUsuario = usuario,
                Contraseña = contraseña,
                Rol = rolPicker.SelectedItem?.ToString() ?? "Usuario"
            };

            await App.Database.GuardarUsuarioAsync(nuevoUsuario);
            await DisplayAlert("Cuenta creada", "Tu usuario ha sido registrado correctamente.", "OK");
            await Navigation.PopAsync(); // Regresa al Login
        }
    }
}
