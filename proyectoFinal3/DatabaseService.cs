using SQLite;
using System.Collections.Generic;

namespace proyectoFinal3;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService(string dbPath)
    {
        // Establecer conexión a la base de datos
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Cliente>().Wait();
    }

    // Método para insertar un cliente
    public Task<int> AddClienteAsync(Cliente cliente)
    {
        return _database.InsertAsync(cliente);
    }

    // Método para obtener todos los clientes
    public Task<List<Cliente>> GetClientesAsync()
    {
        return _database.Table<Cliente>().ToListAsync();
    }

    // Método para autenticar un cliente
    public async Task<Cliente> AuthenticateClienteAsync(string username, string password)
    {
        return await _database.Table<Cliente>()
            .Where(c => c.usuario == username && c.password == password)
            .FirstOrDefaultAsync();
    }
}

// Modelo de Cliente
public class Cliente
{
    [PrimaryKey, AutoIncrement]
    public int idCliente { get; set; }

    public string usuario { get; set; }
    public string password { get; set; }
    public int medico { get; set; }

    // Citas y recetas se almacenan como cadenas separadas por comas
    public string[] citas { get; set; }
    public string recetas { get; set; }
}
public class Administrativo
{
    [PrimaryKey, AutoIncrement]
    public int idAdministrativo { get; set; }

    public string usuario { get; set; }
    public string password { get; set; }
    public string departamento { get; set; }
    public Cliente[] listaClientes { get; set; }

}
