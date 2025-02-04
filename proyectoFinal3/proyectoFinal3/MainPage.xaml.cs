namespace proyectoFinal3
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }


        private async void btnEntrarCliente_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ComprobarCliente());
        }
        private async void btnEntrarAdministrativo_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ComprobarAdministrativo());
        }
    }

}
