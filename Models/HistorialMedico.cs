using SQLite;
using System;

namespace VeterinariaApp.Models
{
    public class HistorialMedico
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NombreMascota { get; set; }
        public DateTime Fecha { get; set; }
        public string Diagnostico { get; set; }
        public string Tratamiento { get; set; }
        public string Observaciones { get; set; }
    }
}
