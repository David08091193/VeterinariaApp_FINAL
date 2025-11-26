using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinariaApp.Models;
using VeterinariaApp.Services;

namespace VeterinariaApp.Views
{
    public partial class AgendaCitasPage : ContentPage
    {
        private readonly MascotaService _mascotaService = new();
        private readonly CitaService _citaService = new();

        public AgendaCitasPage()
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

            await CargarCalendarioDelMes();
        }

        private async Task CargarCalendarioDelMes()
        {
            calendarioGrid.Children.Clear();

            var hoy = DateTime.Today;
            var primerDia = new DateTime(hoy.Year, hoy.Month, 1);
            var ultimoDia = primerDia.AddMonths(1).AddDays(-1);

            var citas = await _citaService.ObtenerTodasLasCitasAsync();
            var citasDelMes = citas
                .Where(c => c.Fecha.Date >= primerDia && c.Fecha.Date <= ultimoDia)
                .Select(c => c.Fecha.Date)
                .Distinct()
                .ToHashSet();

            int diaSemanaInicio = (int)primerDia.DayOfWeek;
            int totalDias = (ultimoDia - primerDia).Days + 1;

            int fila = 0;
            int columna = diaSemanaInicio;

            for (int i = 0; i < totalDias; i++)
            {
                var fecha = primerDia.AddDays(i);
                bool ocupado = citasDelMes.Contains(fecha);
                bool esHoy = fecha.Date == hoy.Date;

                var label = new Label
                {
                    Text = esHoy ? $"Hoy {fecha.Day}" : fecha.Day.ToString(),
                    FontSize = 14,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = esHoy ? Colors.Red : Colors.Black
                };

                var estado = new Label
                {
                    Text = ocupado ? "No disponible" : "Disponible",
                    FontSize = 12,
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = ocupado ? Colors.Gray : Colors.Green
                };

                var stack = new Frame
                {
                    Content = new VerticalStackLayout
                    {
                        Children = { label, estado },
                        Padding = 4
                    },
                    BorderColor = ocupado ? Colors.Gray : Colors.LightGreen,
                    BackgroundColor = ocupado ? Color.FromArgb("#F5F5F5") : Color.FromArgb("#FFFFFF"),
                    CornerRadius = 6,
                    Margin = 2
                };

                calendarioGrid.Add(stack, columna, fila);

                columna++;
                if (columna > 6)
                {
                    columna = 0;
                    fila++;
                }
            }
        }

        private async void OnGuardarCitaClicked(object sender, EventArgs e)
        {
            var mascotaSeleccionada = mascotaPicker.SelectedItem as Mascota;
            DateTime fecha = fechaPicker.Date;
            TimeSpan hora = horaPicker.Time;
            string motivo = motivoEditor.Text?.Trim() ?? string.Empty;

            if (mascotaSeleccionada == null || string.IsNullOrWhiteSpace(motivo))
            {
                await DisplayAlert("Campos incompletos", "Selecciona una mascota y llena el motivo.", "OK");
                return;
            }

            var nuevaCita = new Cita
            {
                Id = 0,
                NombreMascota = mascotaSeleccionada.Nombre,
                Fecha = fecha,
                Hora = hora,
                Motivo = motivo,
                Usuario = Preferences.Get("NombreUsuario", "")
            };

            try
            {
                var citasDelDia = await _citaService.ObtenerCitasPorFechaAsync(fecha);
                bool horarioOcupado = citasDelDia.Any(c => c.Hora == hora);

                if (horarioOcupado)
                {
                    await DisplayAlert("Horario ocupado", "Ya existe una cita en ese horario. Elige otro.", "OK");
                    return;
                }

                bool resultado = await _citaService.CrearCitaAsync(nuevaCita);

                if (resultado)
                {
                    await DisplayAlert("Éxito", "La cita ha sido registrada correctamente.", "OK");

                    mascotaPicker.SelectedItem = null;
                    fechaPicker.Date = DateTime.Today;
                    horaPicker.Time = TimeSpan.Zero;
                    motivoEditor.Text = string.Empty;

                    await CargarCalendarioDelMes();
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo registrar la cita en el servidor.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un problema al guardar la cita: {ex.Message}", "OK");
            }
        }
    }
}
