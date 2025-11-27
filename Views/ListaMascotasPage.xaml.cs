using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
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
                    mascotas = await _mascotaService.ObtenerMascotasAsync();
                }
                else
                {
                    mascotas = await _mascotaService.ObtenerMascotasPorUsuarioAsync(usuario);
                }

                mascotasCollectionView.ItemsSource = mascotas;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudieron cargar las mascotas: {ex.Message}", "OK");
            }
        }

        private async void OnEditarMascotaClicked(object sender, EventArgs e)
        {
            var boton = sender as Button;
            var mascota = boton?.BindingContext as Mascota;
            if (mascota == null) return;

            await Navigation.PushAsync(new EditarMascotaPage(mascota));
        }

        private async void OnEliminarMascotaClicked(object sender, EventArgs e)
        {
            var boton = sender as Button;
            var mascota = boton?.BindingContext as Mascota;
            if (mascota == null) return;

            bool confirmar = await DisplayAlert("Confirmar", $"¿Eliminar a {mascota.Nombre}?", "Sí", "No");
            if (!confirmar) return;

            bool ok = await _mascotaService.EliminarMascotaAsync(mascota.Id);
            if (ok)
            {
                await DisplayAlert("Éxito", "Mascota eliminada correctamente.", "OK");
                await CargarMascotasSegunRol();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo eliminar la mascota.", "OK");
            }
        }
    }
}
