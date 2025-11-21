using Microsoft.Maui.Controls;
using VeterinariaApp.Views;

namespace VeterinariaApp.Views
{
    public partial class MenuPrincipalPage : ContentPage
    {
        public MenuPrincipalPage()
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

        private async void OnAgendaCitasClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AgendaCitasPage());
        }

        // ? Este método abre el formulario para registrar historial médico
        private async void OnRegistrarHistorialClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HistorialMedicoPage());
        }

        // ? Este método abre la pantalla para consultar historial por mascota
        private async void OnVerHistorialClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VerHistorialPage());
        }
        // ? Este método abre la pantalla de ingreso o salida de la mascota
        private async void OnEntradaSalidaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EntradaSalidaPage());
        }
        // ? Este método abre la pantalla de VER ingreso o salida de la mascota

        private async void OnVerEntradaSalidaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VerEntradaSalidaPage());
        }




    }
}
