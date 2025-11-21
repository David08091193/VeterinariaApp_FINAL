using SQLite;

namespace VeterinariaApp.Models
{
    public class EntradaSalida
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string NombreMascota { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public string Motivo { get; set; }
    }
}
