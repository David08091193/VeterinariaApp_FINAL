using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VeterinariaApp.Models;
using VeterinariaApp.Services;

namespace VeterinariaApp.Views
{
    public partial class VerHistorialPage : ContentPage
    {
        private readonly MascotaService _mascotaService = new();
        private readonly HistorialMedicoService _historialService = new();

        public VerHistorialPage()
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

        private async void OnBuscarHistorialClicked(object sender, EventArgs e)
        {
            var mascotaSeleccionada = mascotaPicker.SelectedItem as Mascota;

            if (mascotaSeleccionada == null)
            {
                await DisplayAlert("Campo vacío", "Por favor selecciona una mascota.", "OK");
                return;
            }

            try
            {
                var historial = await _historialService.ObtenerHistorialPorMascotaAsync(mascotaSeleccionada.Nombre);

                if (historial.Count == 0)
                {
                    await DisplayAlert("Sin registros", "No se encontró historial médico para esta mascota.", "OK");
                }

                historialCollectionView.ItemsSource = historial;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo cargar el historial: {ex.Message}", "OK");
            }
        }
    }
}
