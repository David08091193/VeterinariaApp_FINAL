using SQLite;
using System;

namespace VeterinariaApp.Models
{
    public class Cita
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string NombreMascota { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Motivo { get; set; }

        // NUEVO: dueño que agendó
        public string Usuario { get; set; } = string.Empty;
    }
}
