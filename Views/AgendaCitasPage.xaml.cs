using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VeterinariaApp.Models;
using VeterinariaApp.Views;

namespace VeterinariaApp.Views
{
    public partial class AgendaCitasPage : ContentPage
    {
        public AgendaCitasPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Cargar mascotas registradas en el Picker
            var mascotas = await App.Database.ObtenerMascotasAsync();
            mascotaPicker.ItemsSource = mascotas;
        }

        private async void OnGuardarCitaClicked(object sender, EventArgs e)
        {
            var mascotaSeleccionada = mascotaPicker.SelectedItem as Mascota;
            DateTime fecha = fechaPicker.Date;
            TimeSpan hora = horaPicker.Time;
            string motivo = motivoEditor.Text;

            if (mascotaSeleccionada == null || string.IsNullOrWhiteSpace(motivo))
            {
                await DisplayAlert("Campos incompletos", "Por favor selecciona una mascota y llena todos los campos.", "OK");
                return;
            }

            var nuevaCita = new Cita
            {
                NombreMascota = mascotaSeleccionada.Nombre,
                Fecha = fecha,
                Hora = hora,
                Motivo = motivo,
                Usuario = Preferences.Get("NombreUsuario", "") // dueño que agenda
            };

            await App.Database.GuardarCitaAsync(nuevaCita);

            await DisplayAlert("Cita guardada", "La cita ha sido registrada correctamente.", "OK");

            await Navigation.PushAsync(new ListaCitasPage());
        }
    }
}
