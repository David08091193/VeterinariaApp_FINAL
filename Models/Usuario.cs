using SQLite;

namespace VeterinariaApp.Models
{
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public string Rol { get; set; } = "Usuario"; // Por defecto
    }
}
