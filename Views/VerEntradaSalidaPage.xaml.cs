using Microsoft.Maui.Controls;
using System;
using System.Linq;
using VeterinariaApp.Models;
using VeterinariaApp.Services;

namespace VeterinariaApp.Views
{
    public partial class VerEntradaSalidaPage : ContentPage
    {
        private readonly MascotaService _mascotaService = new();
        private readonly EntradaSalidaService _entradaSalidaService = new();

        public VerEntradaSalidaPage()
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

        private async void OnVerRegistrosClicked(object sender, EventArgs e)
        {
            var mascotaSeleccionada = mascotaPicker.SelectedItem as Mascota;

            if (mascotaSeleccionada == null)
            {
                await DisplayAlert("Mascota no seleccionada", "Por favor selecciona una mascota.", "OK");
                return;
            }

            try
            {
                var todosLosRegistros = await _entradaSalidaService.ObtenerRegistrosAsync();
                var registrosFiltrados = todosLosRegistros
                    .Where(r => r.NombreMascota == mascotaSeleccionada.Nombre)
                    .OrderByDescending(r => r.FechaEntrada)
                    .ToList();

                registrosCollectionView.ItemsSource = registrosFiltrados;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudieron cargar los registros: {ex.Message}", "OK");
            }
        }
    }
}
