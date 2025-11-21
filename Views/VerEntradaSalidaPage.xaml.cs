using Microsoft.Maui.Controls;
using System;
using System.Linq;
using VeterinariaApp.Models;

namespace VeterinariaApp.Views
{
    public partial class VerEntradaSalidaPage : ContentPage
    {
        public VerEntradaSalidaPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var mascotas = await App.Database.ObtenerMascotasAsync();
            mascotaPicker.ItemsSource = mascotas;
        }

        private async void OnVerRegistrosClicked(object sender, EventArgs e)
        {
            var mascotaSeleccionada = mascotaPicker.SelectedItem as Mascota;

            if (mascotaSeleccionada == null)
            {
                await DisplayAlert("Mascota no seleccionada", "Por favor selecciona una mascota.", "OK");
                return;
            }

            var todosLosRegistros = await App.Database.ObtenerEntradasSalidasAsync();
            var registrosFiltrados = todosLosRegistros
                .Where(r => r.NombreMascota == mascotaSeleccionada.Nombre)
                .OrderByDescending(r => r.FechaEntrada)
                .ToList();

            registrosCollectionView.ItemsSource = registrosFiltrados;
        }
    }
}
