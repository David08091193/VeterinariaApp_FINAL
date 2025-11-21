using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VeterinariaApp.Models;

namespace VeterinariaApp.Views
{
    public partial class VerHistorialPage : ContentPage
    {
        public VerHistorialPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var mascotas = await App.Database.ObtenerMascotasAsync();
            mascotaPicker.ItemsSource = mascotas;
        }

        private async void OnBuscarHistorialClicked(object sender, EventArgs e)
        {
            var mascotaSeleccionada = mascotaPicker.SelectedItem as Mascota;

            if (mascotaSeleccionada == null)
            {
                await DisplayAlert("Campo vacío", "Por favor selecciona una mascota.", "OK");
                return;
            }

            var historial = await App.Database.ObtenerHistorialPorMascotaAsync(mascotaSeleccionada.Nombre);

            if (historial.Count == 0)
            {
                await DisplayAlert("Sin registros", "No se encontró historial médico para esta mascota.", "OK");
            }

            historialCollectionView.ItemsSource = historial;
        }
    }
}
