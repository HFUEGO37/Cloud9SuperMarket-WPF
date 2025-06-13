using System.Windows;
using Cloud9SuperMarket_WPF.Cloud9SuperMarket.DataAccess;

namespace Cloud9Supermarket.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void IniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasena = pwdContrasena.Password;

            try
            {
                var cliente = ClienteDAO.BuscarPorCredenciales(usuario, contrasena);

                if (cliente != null)
                {
                    if (usuario == "admin@cloud9.com")
                    {
                        var ventanaAdmin = new GestionProductos();
                        ventanaAdmin.Show();
                    }
                    else
                    {
                        var ventanaCliente = new RealizarPedido(cliente);
                        ventanaCliente.Show();
                    }
                    this.Close();
                }
                else
                {
                    txtError.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Registrarse_Click(object sender, RoutedEventArgs e)
        {
            var ventanaRegistro = new RegistroCliente();
            ventanaRegistro.Show();
        }
    }
}


