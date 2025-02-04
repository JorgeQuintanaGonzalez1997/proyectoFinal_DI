using System.Diagnostics;
using Firebase.Database;
using Firebase.Database.Query;
using Google.Cloud.Firestore;
using System.Diagnostics;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using SQLite;


namespace proyectoFinal3;

public partial class ComprobarCliente : ContentPage
{
    private readonly SQLiteAsyncConnection database;

    public ComprobarCliente()
    {
        InitializeComponent();

        // Ruta de la base de datos SQLite
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "clientes.db3");
        database = new SQLiteAsyncConnection(dbPath);

        // Crear la tabla Clientes si no existe
        database.CreateTableAsync<Cliente>().Wait();
    }

    private async void OnBtnLoggin(object sender, EventArgs e)
    {
        var username = UserEntry.Text;
        var password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ErrorLabel.Text = "Por favor ingrese usuario y contrase�a";
            ErrorLabel.IsVisible = true;
            return;
        }

        // Autenticar usuario en la base de datos SQLite
        var user = await AuthenticateUserAsync(username, password);
        Debug.WriteLine("dato usuario: " + username);

        if (user != null)
        {
            Debug.WriteLine("Usuario autenticado correctamente.");
            // Redirige a la siguiente p�gina si la autenticaci�n fue exitosa
            await Navigation.PushAsync(new MainPage());
        }
        else
        {
            // Muestra un error si las credenciales no coinciden
            ErrorLabel.Text = "Usuario o contrase�a incorrectos";
            ErrorLabel.IsVisible = true;
        }
    }
    private async void OnAddTestUserAsync(object sender, EventArgs e)
    {
        var nuevoUsuario = new Cliente
        {
            usuario = "piero1",
            password = "piero1",
            medico = 10,
            citas = "cita1,cita2,cita3",
            recetas = "receta1,receta2,receta3"
        };

        await database.InsertAsync(nuevoUsuario);
        Debug.WriteLine("Usuario agregado exitosamente.");
    }

    private async Task<Cliente> AuthenticateUserAsync(string username, string password)
    {
        try
        {
            // Buscar usuario en la base de datos SQLite
            var user = await database.Table<Cliente>()
                .Where(u => u.usuario == username && u.password == password)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                Debug.WriteLine($"Usuario autenticado: {user.usuario}");
                return user; // Usuario encontrado
            }

            Debug.WriteLine("Usuario o contrase�a incorrectos.");
            return null; // Usuario no encontrado
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al acceder a SQLite: {ex.Message}");
            return null;
        }
    }

    public class Cliente
    {
        [PrimaryKey, AutoIncrement]
        public int idCliente { get; set; }
        public string usuario { get; set; }
        public string password { get; set; }
        public int medico { get; set; }

        // Citas y recetas almacenadas como cadenas separadas por comas
        public string citas { get; set; }
        public string recetas { get; set; }
    }
}