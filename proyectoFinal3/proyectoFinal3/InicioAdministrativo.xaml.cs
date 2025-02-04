//using CloudKit;
using SQLite;
using System.Diagnostics;


namespace proyectoFinal3;

public partial class InicioAdministrativo : ContentPage
{
    private readonly SQLiteAsyncConnection database;
    public InicioAdministrativo()
	{
		InitializeComponent();
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "clientes.db3");
        database = new SQLiteAsyncConnection(dbPath);
    }


    private async void btnBuscarCliente_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Buscar usuario en la base de datos SQLite
            var user = await database.Table<Cliente>()
                .Where(u => u.idCliente.ToString() == entryIdCliente.Text.ToString())
                .FirstOrDefaultAsync();

            if (user != null)
            {
                Debug.WriteLine($"Cliente encontrado: {user.usuario}");
                
            }

            Debug.WriteLine("Cliente no encontrado");
            
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al acceder a SQLite: {ex.Message}");
            
        }
    }
}