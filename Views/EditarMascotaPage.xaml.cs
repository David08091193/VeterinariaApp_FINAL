using Microsoft.Maui.Controls;
using VeterinariaApp.Models;
using VeterinariaApp.Services;

namespace VeterinariaApp.Views
{
    public partial class EditarMascotaPage : ContentPage
    {
        private Mascota mascotaActual;
        private readonly MascotaService _mascotaService = new();

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
            if (string.IsNullOrWhiteSpace(nombreEntry.Text) ||
                string.IsNullOrWhiteSpace(especieEntry.Text) ||
                string.IsNullOrWhiteSpace(razaEntry.Text) ||
                string.IsNullOrWhiteSpace(edadEntry.Text))
            {
                await DisplayAlert("Validación", "Por favor completa todos los campos.", "OK");
                return;
            }

            mascotaActual.Nombre = nombreEntry.Text;
            mascotaActual.Especie = especieEntry.Text;
            mascotaActual.Raza = razaEntry.Text;
            mascotaActual.Edad = edadEntry.Text;

            bool ok = await _mascotaService.ActualizarMascotaAsync(mascotaActual);
            if (ok)
            {
                await DisplayAlert("Éxito", "Mascota actualizada correctamente.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo actualizar la mascota en el servidor.", "OK");
            }
        }
    }
}
