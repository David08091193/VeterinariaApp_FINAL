using VeterinariaApp.Models;
using VeterinariaApp.Services;

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
            var mascotas = await _mascotaService.ObtenerMascotasAsync();
            mascotasCollectionView.ItemsSource = mascotas;
        }
    }
}
