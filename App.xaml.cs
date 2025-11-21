using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System.IO;
using VeterinariaApp.Data;
using VeterinariaApp.Views;

namespace VeterinariaApp
{
    public partial class App : Application
    {
        // Base de datos compartida para mascotas y citas
        public static MascotaDatabase Database { get; private set; }

        public App()
        {
            InitializeComponent();

            // Ruta de la base de datos local
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "Mascotas.db3");

            // Inicializar la base de datos
            Database = new MascotaDatabase(dbPath);

            // Establecer la página principal
            MainPage = new NavigationPage(new BienvenidaPage());
        }
    }
}
