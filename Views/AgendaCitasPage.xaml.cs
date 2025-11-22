using Microsoft.Maui.Controls;
using System;
using VeterinariaApp.Models;
using VeterinariaApp.Services;

namespace VeterinariaApp.Views
{
    public partial class AgendaCitasPage : ContentPage
    {
        private readonly MascotaService _mascotaService = new();
        private readonly CitaService _citaService = new();

        public AgendaCitasPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var mascotas = await _mascotaService.ObtenerMascotasAsync();
                mascotaPicker.ItemsSource = mascotas;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudieron cargar las mascotas: {ex.Message}", "OK");
            }
        }

        private async void OnGuardarCitaClicked(object sender, EventArgs e)
        {
            var mascotaSeleccionada = mascotaPicker.SelectedItem as Mascota;
            DateTime fecha = fechaPicker.Date;
            TimeSpan hora = horaPicker.Time;
            string motivo = motivoEditor.Text?.Trim() ?? string.Empty;

            if (mascotaSeleccionada == null || string.IsNullOrWhiteSpace(motivo))
            {
                await DisplayAlert("Campos incompletos", "Selecciona una mascota y llena el motivo.", "OK");
                return;
            }

            var nuevaCita = new Cita
            {
                Id = 0,
                NombreMascota = mascotaSeleccionada.Nombre,
                Fecha = fecha,
                Hora = hora,
                Motivo = motivo,
                Usuario = Preferences.Get("NombreUsuario", "") // dueño que agenda
            };

            try
            {
                bool resultado = await _citaService.CrearCitaAsync(nuevaCita);

                if (resultado)
                {
                    await DisplayAlert("Éxito", "La cita ha sido registrada correctamente.", "OK");
                    // Limpieza rápida
                    mascotaPicker.SelectedItem = null;
                    fechaPicker.Date = DateTime.Today;
                    horaPicker.Time = TimeSpan.Zero;
                    motivoEditor.Text = string.Empty;

                    // Opcional: navegar a la lista
                    await Navigation.PushAsync(new ListaCitasPage());
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo registrar la cita en el servidor.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un problema al guardar la cita: {ex.Message}", "OK");
            }
        }
    }
}
