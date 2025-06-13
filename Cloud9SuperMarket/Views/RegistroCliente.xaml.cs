using System.Windows;
using Cloud9SuperMarket_WPF.Cloud9SuperMarket.Models;
using Cloud9SuperMarket_WPF.Cloud9SuperMarket.DataAccess;

namespace Cloud9Supermarket.Views
{
    public partial class RegistroCliente : Window
    {
        public RegistroCliente()
        {
            InitializeComponent();
        }

        private void Registrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validación de campos vacíos
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtApellido.Text) ||
                    string.IsNullOrWhiteSpace(txtDni.Text) ||
                    string.IsNullOrWhiteSpace(txtCorreo.Text) ||
                    string.IsNullOrWhiteSpace(txtTelefono.Text))
                {
                    MessageBox.Show("Por favor complete todos los campos.", "Advertencia",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Creación del objeto Persona
                var persona = new Persona(
                    txtNombre.Text.Trim(),
                    txtApellido.Text.Trim(),
                    txtDni.Text.Trim(),
                    txtCorreo.Text.Trim(),
                    txtTelefono.Text.Trim()
                );

                // Registro del cliente
                int idCliente = ClienteDAO.RegistrarCliente(persona);

                MessageBox.Show($"Cliente registrado con éxito. ID: {idCliente}",
                    "Registro Exitoso",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) when (ex.Number == 1062) // Código de error para violación de clave única en MySQL
            {
                MessageBox.Show("El DNI o correo electrónico ya están registrados.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar cliente: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}