using Microsoft.Maui.Controls;
using VeterinariaApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace VeterinariaApp.Views
{
    public partial class ListaMascotasPage : ContentPage
    {
        private List<Mascota> todasLasMascotas;

        public ListaMascotasPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            todasLasMascotas = await App.Database.ObtenerMascotasAsync();
            mascotasCollection.ItemsSource = todasLasMascotas;
        }

        private void OnBusquedaTextChanged(object sender, TextChangedEventArgs e)
        {
            string texto = e.NewTextValue?.ToLower() ?? "";

            var filtradas = todasLasMascotas.Where(m =>
                (m.Nombre?.ToLower().Contains(texto) ?? false) ||
                (m.Especie?.ToLower().Contains(texto) ?? false)).ToList();

            mascotasCollection.ItemsSource = filtradas;
        }

        private async void OnEliminarClicked(object sender, EventArgs e)
        {
            var boton = sender as Button;
            var mascota = boton?.CommandParameter as Mascota;

            if (mascota != null)
            {
                bool confirmar = await DisplayAlert("Confirmar", $"¿Eliminar a {mascota.Nombre}?", "Sí", "No");
                if (confirmar)
                {
                    await App.Database.EliminarMascotaAsync(mascota);
                    todasLasMascotas = await App.Database.ObtenerMascotasAsync();
                    mascotasCollection.ItemsSource = todasLasMascotas;
                }
            }
        }

        private async void OnEditarClicked(object sender, EventArgs e)
        {
            var boton = sender as Button;
            var mascota = boton?.CommandParameter as Mascota;

            if (mascota != null)
            {
                await Navigation.PushAsync(new EditarMascotaPage(mascota));
            }
        }
    }
}
