using Microsoft.Maui.Controls;
using VeterinariaApp.Models;

namespace VeterinariaApp.Views
{
    public partial class EditarMascotaPage : ContentPage
    {
        private Mascota mascotaActual;

        public EditarMascotaPage(Mascota mascota)
        {
            InitializeComponent();
            mascotaActual = mascota;

            nombreEntry.Text = mascota.Nombre;
            especieEntry.Text = mascota.Especie;
            razaEntry.Text = mascota.Raza;
            edadEntry.Text = mascota.Edad;

            if (!string.IsNullOrWhiteSpace(mascota.FotoPath))
            {
                fotoMascota.Source = ImageSource.FromFile(mascota.FotoPath);
                fotoMascota.IsVisible = true;
            }
        }

        private async void OnActualizarClicked(object sender, EventArgs e)
        {
            mascotaActual.Nombre = nombreEntry.Text;
            mascotaActual.Especie = especieEntry.Text;
            mascotaActual.Raza = razaEntry.Text;
            mascotaActual.Edad = edadEntry.Text;

            await App.Database.GuardarMascotaAsync(mascotaActual);
            await DisplayAlert("Éxito", "Mascota actualizada correctamente.", "OK");
            await Navigation.PopAsync(); // Regresa a la lista
        }
    }
}
