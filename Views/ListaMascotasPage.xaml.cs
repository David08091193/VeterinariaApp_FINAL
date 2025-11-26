using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.Threading.Tasks;
using VeterinariaApp.Services;
using VeterinariaApp.Models;

namespace VeterinariaApp.Views
{
    public partial class ListaMascotasPage : ContentPage
    {
        private readonly MascotaService _mascotaService = new();

        public ListaMascotasPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarMascotasSegunRol();
        }

        private async Task CargarMascotasSegunRol()
        {
            try
            {
                string rol = Preferences.Get("Rol", "");
                string usuario = Preferences.Get("NombreUsuario", "");

                List<Mascota> mascotas;

                if (rol == "Administrador")
                {
                    // Ver todas las mascotas
                    mascotas = await _mascotaService.ObtenerMascotasAsync();
                }
                else
                {
                    // Ver solo las del usuario actual
                    mascotas = await _mascotaService.ObtenerMascotasPorUsuarioAsync(usuario);
                }

                mascotasCollectionView.ItemsSource = mascotas;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudieron cargar las mascotas: {ex.Message}", "OK");
            }
        }
    }
}
