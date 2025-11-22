using Microsoft.Maui.Controls;
using System;
using VeterinariaApp.Models;
using VeterinariaApp.Services;

namespace VeterinariaApp.Views
{
    public partial class HistorialMedicoPage : ContentPage
    {
        private readonly MascotaService _mascotaService = new();
        private readonly HistorialMedicoService _historialService = new();

        public HistorialMedicoPage()
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

        private async void OnGuardarHistorialClicked(object sender, EventArgs e)
        {
            var mascotaSeleccionada = mascotaPicker.SelectedItem as Mascota;
            string diagnostico = descripcionEditor.Text;
            string tratamiento = tratamientoEditor.Text;
            string observaciones = observacionesEditor.Text;
            DateTime fecha = fechaPicker.Date;

            if (mascotaSeleccionada == null ||
                string.IsNullOrWhiteSpace(diagnostico) ||
                string.IsNullOrWhiteSpace(tratamiento))
            {
                await DisplayAlert("Campos incompletos", "Selecciona una mascota y llena los campos obligatorios.", "OK");
                return;
            }

            var historial = new HistorialMedico
            {
                NombreMascota = mascotaSeleccionada.Nombre,
                Fecha = fecha,
                Diagnostico = diagnostico,
                Tratamiento = tratamiento,
                Observaciones = observaciones
            };

            bool ok = await _historialService.CrearHistorialAsync(historial);

            if (ok)
            {
                await DisplayAlert("Éxito", "El historial médico ha sido registrado correctamente.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo registrar el historial médico en el servidor.", "OK");
            }
        }
    }
}
