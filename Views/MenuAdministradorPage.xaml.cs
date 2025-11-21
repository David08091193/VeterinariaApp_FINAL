using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace VeterinariaApp.Views
{
    public partial class MenuAdministradorPage : ContentPage
    {
        public MenuAdministradorPage()
        {
            InitializeComponent();
        }

        private async void OnRegistrarMascotaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistroMascotaPage());
        }

        private async void OnListaMascotasClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaMascotasPage());
        }

        private async void OnAgendaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AgendaCitasPage());
        }

        private async void OnCerrarSesionClicked(object sender, EventArgs e)
        {
            Preferences.Clear();
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
