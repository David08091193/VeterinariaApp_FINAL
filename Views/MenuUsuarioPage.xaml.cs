using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace VeterinariaApp.Views
{
    public partial class MenuUsuarioPage : ContentPage
    {
        public MenuUsuarioPage()
        {
            InitializeComponent();
        }

        private async void OnMisMascotasClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaMascotasPage());
        }

        private async void OnAgendarClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AgendaCitasPage());
        }

        private async void OnMisCitasClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaCitasPage());
        }

        private async void OnCerrarSesionClicked(object sender, EventArgs e)
        {
            Preferences.Clear();
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
