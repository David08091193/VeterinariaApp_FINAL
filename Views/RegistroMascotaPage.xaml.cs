using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;
using System;
using VeterinariaApp.Models;

namespace VeterinariaApp.Views
{
    public partial class RegistroMascotaPage : ContentPage
    {
        // Cambia la declaración del campo fotoFile para que acepte valores null
        private FileResult? fotoFile;

        public RegistroMascotaPage()
        {
            InitializeComponent();
        }

        private async void OnTomarFotoClicked(object sender, EventArgs e)
        {
            try
            {
                var foto = await MediaPicker.CapturePhotoAsync();
                if (foto != null)
                {
                    fotoFile = foto;
                    var stream = await foto.OpenReadAsync();
                    fotoMascota.Source = ImageSource.FromStream(() => stream);
                    fotoMascota.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo tomar la foto: {ex.Message}", "OK");
            }
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            string nombre = string.IsNullOrWhiteSpace(nombreEntry.Text) ? "" : nombreEntry.Text;
            string especie = string.IsNullOrWhiteSpace(especieEntry.Text) ? "" : especieEntry.Text;
            string raza = string.IsNullOrWhiteSpace(razaEntry.Text) ? "" : razaEntry.Text;
            string edad = string.IsNullOrWhiteSpace(edadEntry.Text) ? "" : edadEntry.Text;

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(especie) ||
                string.IsNullOrWhiteSpace(raza) ||
                string.IsNullOrWhiteSpace(edad))
            {
                await DisplayAlert("Validación", "Por favor completa todos los campos.", "OK");
                return;
            }

            var nuevaMascota = new Mascota
            {
                Nombre = nombre,
                Especie = especie,
                Raza = raza,
                Edad = edad,
                FotoPath = fotoFile?.FullPath
            };

            await App.Database.GuardarMascotaAsync(nuevaMascota);

            await DisplayAlert("Éxito", "Mascota registrada correctamente.", "OK");

            nombreEntry.Text = "";
            especieEntry.Text = "";
            razaEntry.Text = "";
            edadEntry.Text = "";
            fotoMascota.Source = null;
            fotoMascota.IsVisible = false;
        }

        private async void OnVerMascotasClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaMascotasPage());
        }
    }
}
