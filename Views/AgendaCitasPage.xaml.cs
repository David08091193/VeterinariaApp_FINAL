using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
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
                    FontSize = 13,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = esHoy ? Colors.Red : Colors.Black
                };

                var estado = new Label
                {
                    Text = ocupado ? "No disponible" : "Disponible",
                    FontSize = 11,
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = ocupado ? Colors.Gray : Colors.Green,
                    LineBreakMode = LineBreakMode.WordWrap,
                    MaxLines = 2
                };

                var stack = new Frame
                {
                    Content = new VerticalStackLayout
                    {
                        Children = { label, estado },
                        Padding = 3
                    },
                    BorderColor = ocupado ? Colors.Gray : Colors.LightGreen,
                    BackgroundColor = ocupado ? Color.FromArgb("#F5F5F5") : Color.FromArgb("#FFFFFF"),
                    CornerRadius = 4,
                    Margin = 1,
                    MinimumHeightRequest = 50
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

        private async void OnDetectarUbicacionClicked(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLocationAsync();
                if (location == null)
                {
                    await DisplayAlert("Ubicación", "No se pudo obtener la ubicación actual.", "OK");
                    return;
                }

                string direccion = await ReverseGeocodeAsync(location);
                if (!string.IsNullOrWhiteSpace(direccion))
                {
                    await DisplayAlert("Ubicación", direccion, "OK");
                }
                else
                {
                    await DisplayAlert("Ubicación",
                        $"Latitud: {location.Latitude}\nLongitud: {location.Longitude}",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo obtener la ubicación: {ex.Message}", "OK");
            }
        }

        private async Task<string> ReverseGeocodeAsync(Location location)
        {
#if WINDOWS
            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.UserAgent.ParseAdd("VeterinariaApp/1.0 (contacto: ejemplo@correo.com)");

                string url = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={location.Latitude}&lon={location.Longitude}";
                var resp = await http.GetAsync(url);
                if (!resp.IsSuccessStatusCode) return string.Empty;

                var json = await resp.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(json);
                if (!doc.RootElement.TryGetProperty("address", out var addr)) return string.Empty;

                string ciudad = Get(addr, "city", "town", "village");
                string barrio = Get(addr, "suburb", "neighbourhood");
                string calle  = Get(addr, "road");
                string estado = Get(addr, "state");
                string pais   = Get(addr, "country");

                return $"Ubicación detectada:\nCiudad: {ciudad}\nBarrio: {barrio}\nCalle: {calle}\nProvincia/Estado: {estado}\nPaís: {pais}";
            }
            catch
            {
                return string.Empty;
            }
#else
            try
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(location);
                var p = placemarks?.FirstOrDefault();
                if (p == null) return string.Empty;

                string ciudad = p.Locality;
                string barrio = p.SubLocality;
                string calle = p.Thoroughfare;
                string estado = p.AdminArea;
                string pais = p.CountryName;

                return $"Ubicación detectada:\nCiudad: {ciudad}\nBarrio: {barrio}\nCalle: {calle}\nProvincia/Estado: {estado}\nPaís: {pais}";
            }
            catch
            {
                return string.Empty;
            }
#endif
        }

        private static string Get(JsonElement addr, params string[] keys)
        {
            foreach (var k in keys)
            {
                if (addr.TryGetProperty(k, out var v))
                {
                    var s = v.GetString();
                    if (!string.IsNullOrWhiteSpace(s)) return s;
                }
            }
            return "(no disponible)";
        }
    }
}
