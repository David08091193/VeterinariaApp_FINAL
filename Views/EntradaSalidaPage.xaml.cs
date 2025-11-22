using Microsoft.Maui.Controls;
using System;
using VeterinariaApp.Models;
using VeterinariaApp.Services;

namespace VeterinariaApp.Views
{
    public partial class EntradaSalidaPage : ContentPage
    {
        private readonly MascotaService _mascotaService = new();
        private readonly EntradaSalidaService _entradaSalidaService = new();

        public EntradaSalidaPage()
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

        private async void OnGuardarEntradaSalidaClicked(object sender, EventArgs e)
        {
            var mascotaSeleccionada = mascotaPicker.SelectedItem as Mascota;
            var motivo = motivoEditor.Text;

            if (mascotaSeleccionada == null || string.IsNullOrWhiteSpace(motivo))
            {
                await DisplayAlert("Campos incompletos", "Selecciona una mascota y escribe el motivo.", "OK");
                return;
            }

            var fechaEntrada = fechaEntradaPicker.Date + horaEntradaPicker.Time;
            var fechaSalida = fechaSalidaPicker.Date + horaSalidaPicker.Time;

            var registro = new EntradaSalida
            {
                NombreMascota = mascotaSeleccionada.Nombre,
                FechaEntrada = fechaEntrada,
                FechaSalida = fechaSalida,
                Motivo = motivo
            };

            bool ok = await _entradaSalidaService.CrearRegistroAsync(registro);

            if (ok)
            {
                await DisplayAlert("Éxito", "El ingreso/salida ha sido registrado correctamente.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo registrar el ingreso/salida en el servidor.", "OK");
            }
        }
    }
}
