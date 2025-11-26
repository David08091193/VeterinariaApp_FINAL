using Microsoft.Maui.Controls;
using System;
using VeterinariaApp.Models;
using VeterinariaApp.Services;

namespace VeterinariaApp.Views
{
    public partial class EditarCitaPage : ContentPage
    {
        private readonly CitaService _citaService = new();
        private readonly Cita _citaOriginal;

        public EditarCitaPage(Cita cita)
        {
            InitializeComponent();
            _citaOriginal = cita;

            fechaPicker.Date = cita.Fecha;
            horaPicker.Time = cita.Hora;
            motivoEditor.Text = cita.Motivo;
        }

        private async void OnGuardarCambiosClicked(object sender, EventArgs e)
        {
            _citaOriginal.Fecha = fechaPicker.Date;
            _citaOriginal.Hora = horaPicker.Time;
            _citaOriginal.Motivo = motivoEditor.Text?.Trim() ?? "";

            bool ok = await _citaService.ActualizarCitaAsync(_citaOriginal);

            if (ok)
            {
                await DisplayAlert("Éxito", "La cita fue actualizada correctamente.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo actualizar la cita.", "OK");
            }
        }
    }
}
