using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace VeterinariaApp.Views
{
    public partial class MenuVeterinarioPage : ContentPage
    {
        public MenuVeterinarioPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Aplicar estilos dinámicos si deseas cambiar en tiempo real
            btnHistorial.Style = (Style)Application.Current.Resources["VetButtonStyle"];
            btnAgenda.Style = (Style)Application.Current.Resources["VetButtonStyle"];
            btnLista.Style = (Style)Application.Current.Resources["VetButtonStyle"];
        }

        private async void OnHistorialClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HistorialMedicoPage());
        }

        private async void OnAgendaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AgendaCitasPage());
        }

        private async void OnListaCitasClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaCitasPage());
        }

        private async void OnCerrarSesionClicked(object sender, EventArgs e)
        {
            Preferences.Clear(); // Limpia sesión
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
