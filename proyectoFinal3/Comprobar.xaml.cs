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

public partial class Comprobar : ContentPage
{
    private readonly SQLiteAsyncConnection database;

    public Comprobar()
    {
        InitializeComponent();

        // Ruta de la base de datos SQLite
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "clientes.db3");
        database = new SQLiteAsyncConnection(dbPath);

        // Crear la tabla Clientes si no existe
        database.CreateTableAsync<User>().Wait();
    }

    private async void OnBtnLoggin(object sender, EventArgs e)
    {
        var username = UserEntry.Text;
        var password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ErrorLabel.Text = "Por favor ingrese usuario y contraseña";
            ErrorLabel.IsVisible = true;
            return;
        }

        // Autenticar usuario en la base de datos SQLite
        var user = await AuthenticateUserAsync(username, password);
        Debug.WriteLine("dato usuario: " + username);

        if (user != null)
        {
            Debug.WriteLine("Usuario autenticado correctamente.");
            // Redirige a la siguiente página si la autenticación fue exitosa
            await Navigation.PushAsync(new MainPage());
        }
        else
        {
            // Muestra un error si las credenciales no coinciden
            ErrorLabel.Text = "Usuario o contraseña incorrectos";
            ErrorLabel.IsVisible = true;
        }
    }
    private async void OnAddTestUserAsync(object sender, EventArgs e)
    {
        var nuevoUsuario = new User
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

    private async Task<User> AuthenticateUserAsync(string username, string password)
    {
        try
        {
            // Buscar usuario en la base de datos SQLite
            var user = await database.Table<User>()
                .Where(u => u.usuario == username && u.password == password)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                Debug.WriteLine($"Usuario autenticado: {user.usuario}");
                return user; // Usuario encontrado
            }

            Debug.WriteLine("Usuario o contraseña incorrectos.");
            return null; // Usuario no encontrado
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al acceder a SQLite: {ex.Message}");
            return null;
        }
    }

    public class User
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