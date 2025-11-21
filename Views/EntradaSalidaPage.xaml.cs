using Microsoft.Maui.Controls;
using System;
using VeterinariaApp.Models;

namespace VeterinariaApp.Views
{
    public partial class EntradaSalidaPage : ContentPage
    {
        public EntradaSalidaPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var mascotas = await App.Database.ObtenerMascotasAsync();
            mascotaPicker.ItemsSource = mascotas;
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

            await App.Database.GuardarEntradaSalidaAsync(registro);
            await DisplayAlert("Registro guardado", "El ingreso/salida ha sido registrado correctamente.", "OK");
            await Navigation.PopAsync();
        }
    }
}
