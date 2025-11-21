using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using VeterinariaApp.Models;

namespace VeterinariaApp.Views
{
    public partial class HistorialMedicoPage : ContentPage
    {
        public HistorialMedicoPage()
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
                await DisplayAlert("Campos incompletos", "Por favor selecciona una mascota y llena los campos obligatorios.", "OK");
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

            await App.Database.GuardarHistorialAsync(historial);
            await DisplayAlert("Historial guardado", "El historial médico ha sido registrado correctamente.", "OK");
            await Navigation.PopAsync();
        }
    }
}
