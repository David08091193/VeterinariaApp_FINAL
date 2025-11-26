using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage; // <-- Importante para usar Preferences
using System;
using VeterinariaApp.Models;
using VeterinariaApp.Services;

namespace VeterinariaApp.Views
{
    public partial class RegistroMascotaPage : ContentPage
    {
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
                Id = 0,
                Nombre = nombre,
                Especie = especie,
                Raza = raza,
                Edad = edad,
                FotoPath = fotoFile?.FullPath ?? "sin-foto.jpg",
                Usuario = Preferences.Get("NombreUsuario", "") // Aquí asociamos la mascota al usuario actual
            };

            var servicio = new MascotaService();
            bool resultado = await servicio.CrearMascotaAsync(nuevaMascota);

            if (resultado)
            {
                await DisplayAlert("Éxito", "Mascota registrada correctamente en el servidor.", "OK");

                nombreEntry.Text = "";
                especieEntry.Text = "";
                razaEntry.Text = "";
                edadEntry.Text = "";
                fotoMascota.Source = null;
                fotoMascota.IsVisible = false;
            }
            else
            {
                await DisplayAlert("Error", "No se pudo registrar la mascota en el servidor.", "OK");
            }
        }

        private async void OnVerMascotasClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaMascotasPage());
        }
    }
}
