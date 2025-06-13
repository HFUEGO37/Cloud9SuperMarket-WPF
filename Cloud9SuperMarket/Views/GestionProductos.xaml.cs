using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using Cloud9SuperMarket_WPF.Cloud9SuperMarket.DataAccess;
using Cloud9SuperMarket_WPF.Cloud9SuperMarket.Models;


namespace Cloud9Supermarket.Views
{
    public partial class GestionProductos : Window
    {
        private ObservableCollection<Producto> _productos;
        private const string CONNECTION_STRING_NAME = "Cloud9SupermarketDB";


        public GestionProductos()
        {
            InitializeComponent();
            _productos = new ObservableCollection<Producto>();
            dgProductos.ItemsSource = _productos;
            CargarProductos();
        }


        private async void CargarProductos()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                await Task.Run(() =>
                {
                    var productosBD = ProductoDAO.ObtenerTodos();
                    Dispatcher.Invoke(() =>
                    {
                        _productos.Clear();
                        foreach (var p in productosBD)
                        {
                            _productos.Add(p);
                        }
                    });
                });
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error de base de datos al cargar productos: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }


        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtCategoria.Text) ||
                    string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                    string.IsNullOrWhiteSpace(txtStock.Text))
                {
                    MessageBox.Show("Completa todos los campos.", "Advertencia",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                if (!double.TryParse(txtPrecio.Text, out double precio) || precio <= 0)
                {
                    MessageBox.Show("Ingrese un precio válido.", "Advertencia",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
                {
                    MessageBox.Show("Ingrese un stock válido.", "Advertencia",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                Producto nuevo = new Producto
                {
                    Nombre = txtNombre.Text,
                    Categoria = txtCategoria.Text,
                    Precio = precio,
                    Stock = stock
                };


                // Insertar en BD
                using (var connection = new DatabaseConnection())
                {
                    var command = connection.CreateCommand(
                        "INSERT INTO Productos (Nombre, Categoria, Precio, Stock) " +
                        "OUTPUT INSERTED.IdProducto " +
                        "VALUES (@nombre, @categoria, @precio, @stock)");

                    command.Parameters.AddWithValue("@nombre", nuevo.Nombre);
                    command.Parameters.AddWithValue("@categoria", nuevo.Categoria);
                    command.Parameters.AddWithValue("@precio", nuevo.Precio);
                    command.Parameters.AddWithValue("@stock", nuevo.Stock);

                    nuevo.IdProducto = (int)command.ExecuteScalar();
                }


                _productos.Add(nuevo);
                LimpiarCampos();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error de base de datos al agregar producto: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar producto: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgProductos.SelectedItem is Producto seleccionado)
                {
                    if (!double.TryParse(txtPrecio.Text, out double precio) || precio <= 0)
                    {
                        MessageBox.Show("Ingrese un precio válido.", "Advertencia",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }


                    if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
                    {
                        MessageBox.Show("Ingrese un stock válido.", "Advertencia",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }


                    // Actualizar en BD
                    using (var connection = new DatabaseConnection())
                    {
                        var command = connection.CreateCommand(
                            "UPDATE Productos SET Nombre = @nombre, Categoria = @categoria, " +
                            "Precio = @precio, Stock = @stock WHERE IdProducto = @id");

                        command.Parameters.AddWithValue("@nombre", txtNombre.Text);
                        command.Parameters.AddWithValue("@categoria", txtCategoria.Text);
                        command.Parameters.AddWithValue("@precio", precio);
                        command.Parameters.AddWithValue("@stock", stock);
                        command.Parameters.AddWithValue("@id", seleccionado.IdProducto);

                        command.ExecuteNonQuery();
                    }


                    seleccionado.Nombre = txtNombre.Text;
                    seleccionado.Categoria = txtCategoria.Text;
                    seleccionado.Precio = precio;
                    seleccionado.Stock = stock;


                    dgProductos.Items.Refresh();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("Seleccione un producto para actualizar.", "Advertencia",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error de base de datos al actualizar producto: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar producto: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgProductos.SelectedItem is Producto seleccionado)
                {
                    if (MessageBox.Show($"¿Eliminar el producto {seleccionado.Nombre}?",
                        "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        // Eliminar de BD
                        using (var connection = new DatabaseConnection())
                        {
                            var command = connection.CreateCommand(
                                "DELETE FROM Productos WHERE IdProducto = @id");
                            command.Parameters.AddWithValue("@id", seleccionado.IdProducto);
                            command.ExecuteNonQuery();
                        }


                        _productos.Remove(seleccionado);
                        LimpiarCampos();
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un producto para eliminar.", "Advertencia",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (MySqlException ex) when (ex.Number == 547) // FK violation
            {
                MessageBox.Show("No se puede eliminar el producto porque está incluido en pedidos.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar producto: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }


        private void CargarProductos_Click(object sender, RoutedEventArgs e)
        {
            CargarProductos();
        }


        private void dgProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProductos.SelectedItem is Producto seleccionado)
            {
                txtIdProducto.Text = seleccionado.IdProducto.ToString();
                txtNombre.Text = seleccionado.Nombre;
                txtCategoria.Text = seleccionado.Categoria;
                txtPrecio.Text = seleccionado.Precio.ToString("0.00");
                txtStock.Text = seleccionado.Stock.ToString();
            }
        }


        private void LimpiarCampos()
        {
            txtIdProducto.Text = "";
            txtNombre.Text = "";
            txtCategoria.Text = "";
            txtPrecio.Text = "";
            txtStock.Text = "";
            dgProductos.UnselectAll();
        }
    }
}


