using SQLite;
using System.Collections.Generic;
using System.Linq;              // NECESARIO para Where/OrderBy
using System.Threading.Tasks;
using VeterinariaApp.Models;

namespace VeterinariaApp.Data
{
    public class MascotaDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public MascotaDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Mascota>().Wait();
            _database.CreateTableAsync<Cita>().Wait();
            _database.CreateTableAsync<HistorialMedico>().Wait();
            _database.CreateTableAsync<EntradaSalida>().Wait();
            _database.CreateTableAsync<Usuario>().Wait();
        }

        public Task<int> GuardarMascotaAsync(Mascota mascota)
        {
            return _database.InsertAsync(mascota);
        }

        public Task<List<Mascota>> ObtenerMascotasAsync()
        {
            return _database.Table<Mascota>().ToListAsync();
        }

        public Task<int> EliminarMascotaAsync(Mascota mascota)
        {
            return _database.DeleteAsync(mascota);
        }

        // Guardar una cita
        public Task<int> GuardarCitaAsync(Cita cita)
        {
            return _database.InsertAsync(cita);
        }

        // Obtener todas las citas (general)
        public Task<List<Cita>> ObtenerCitasAsync()
        {
            return _database.Table<Cita>().OrderBy(c => c.Fecha).ToListAsync();
        }

        // NUEVO: Veterinario ve todas las citas
        public Task<List<Cita>> ObtenerTodasLasCitasAsync()
        {
            return _database.Table<Cita>()
                            .OrderBy(c => c.Fecha)
                            .ToListAsync();
        }

        // NUEVO: Usuario ve solo sus citas
        public Task<List<Cita>> ObtenerCitasPorUsuarioAsync(string nombreUsuario)
        {
            return _database.Table<Cita>()
                            .Where(c => c.Usuario == nombreUsuario)
                            .OrderBy(c => c.Fecha)
                            .ToListAsync();
        }

        // Eliminar una cita
        public Task<int> EliminarCitaAsync(Cita cita)
        {
            return _database.DeleteAsync(cita);
        }

        // Guardar historial médico
        public Task<int> GuardarHistorialAsync(HistorialMedico historial)
        {
            return _database.InsertAsync(historial);
        }

        public Task<List<HistorialMedico>> ObtenerHistorialPorMascotaAsync(string nombreMascota)
        {
            return _database.Table<HistorialMedico>()
                            .Where(h => h.NombreMascota.ToLower() == nombreMascota.ToLower())
                            .OrderByDescending(h => h.Fecha)
                            .ToListAsync();
        }

        // Registro entrada salida
        public Task<int> GuardarEntradaSalidaAsync(EntradaSalida registro)
        {
            return _database.InsertAsync(registro);
        }

        public Task<List<EntradaSalida>> ObtenerEntradasSalidasAsync()
        {
            return _database.Table<EntradaSalida>().ToListAsync();
        }

        // Guardar y validar usuario
        public Task<int> GuardarUsuarioAsync(Usuario usuario)
        {
            return _database.InsertAsync(usuario);
        }

        public async Task<Usuario> ValidarUsuarioAsync(string nombreUsuario, string contraseña)
        {
            return await _database.Table<Usuario>()
                .Where(u => u.NombreUsuario == nombreUsuario && u.Contraseña == contraseña)
                .FirstOrDefaultAsync();
        }



    }
}
