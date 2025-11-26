using SQLite;

namespace VeterinariaApp.Models
{
    public class Mascota
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public string Edad { get; set; }

        public string FotoPath { get; set; }

        // Nueva propiedad para asociar la mascota al usuario que la registró
        public string Usuario { get; set; }
    }
}
