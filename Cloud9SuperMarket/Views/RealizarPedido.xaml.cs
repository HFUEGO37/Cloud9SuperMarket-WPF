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
    public partial class RealizarPedido : Window
    {
        private ObservableCollection<Producto> _productosDisponibles;
        private ObservableCollection<LineaPedido> _productosPedido;
        private Cliente _cliente;


        public RealizarPedido(Cliente cliente)
        {
            InitializeComponent();
            _cliente = cliente;
            _productosDisponibles = new ObservableCollection<Producto>();
            _productosPedido = new ObservableCollection<LineaPedido>();
            dgDisponibles.ItemsSource = _productosDisponibles;
            dgPedido.ItemsSource = _productosPedido;
            CargarProductosDisponibles();
        }


        private async void CargarProductosDisponibles()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                await Task.Run(() =>
                {
                    var productosBD = ProductoDAO.ObtenerTodos();
                    Dispatcher.Invoke(() =>
                    {
                        _productosDisponibles.Clear();
                        foreach (var p in productosBD)
                        {
                            _productosDisponibles.Add(p);
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


        private void AgregarAlPedido_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgDisponibles.SelectedItem is Producto productoSeleccionado)
                {
                    if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
                    {
                        MessageBox.Show("Ingrese una cantidad válida.", "Advertencia",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }


                    if (cantidad > productoSeleccionado.Stock)
                    {
                        MessageBox.Show("No hay suficiente stock disponible.", "Advertencia",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }


                    var existente = _productosPedido.FirstOrDefault(p => p.IdProducto == productoSeleccionado.IdProducto);


                    if (existente != null)
                    {
                        existente.Cantidad += cantidad;
                    }
                    else
                    {
                        _productosPedido.Add(new LineaPedido
                        {
                            IdProducto = productoSeleccionado.IdProducto,
                            Nombre = productoSeleccionado.Nombre,
                            Cantidad = cantidad,
                            PrecioUnitario = productoSeleccionado.Precio
                        });
                    }


                    productoSeleccionado.Stock -= cantidad;
                    dgDisponibles.Items.Refresh();
                    dgPedido.Items.Refresh();
                    ActualizarTotal();
                }
                else
                {
                    MessageBox.Show("Seleccione un producto para agregar.", "Advertencia",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar producto al pedido: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void QuitarDelPedido_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgPedido.SelectedItem is LineaPedido lineaSeleccionada)
                {
                    var producto = _productosDisponibles.FirstOrDefault(p => p.IdProducto == lineaSeleccionada.IdProducto);
                    if (producto != null)
                    {
                        producto.Stock += lineaSeleccionada.Cantidad;
                        dgDisponibles.Items.Refresh();
                    }


                    _productosPedido.Remove(lineaSeleccionada);
                    ActualizarTotal();
                }
                else
                {
                    MessageBox.Show("Seleccione un producto para quitar.", "Advertencia",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al quitar producto del pedido: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ConfirmarPedido_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_productosPedido.Count == 0)
                {
                    MessageBox.Show("No hay productos en el pedido.", "Advertencia",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                double total = _productosPedido.Sum(p => p.Subtotal);

                if (MessageBox.Show($"¿Confirmar pedido por un total de {total:C2}?", "Confirmar",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (var connection = new DatabaseConnection())
                    {
                        // Iniciar transacción
                        var transaction = connection.BeginTransaction();

                        try
                        {
                            // Insertar pedido
                            var command = connection.CreateCommand(
                                "INSERT INTO Pedidos (IdCliente, Total) " +
                                "VALUES (@idCliente, @total); " +
                                "SELECT LAST_INSERT_ID();");

                            command.Parameters.AddWithValue("@idCliente", _cliente.IdCliente);
                            command.Parameters.AddWithValue("@total", total);

                            int idPedido = Convert.ToInt32(command.ExecuteScalar());

                            // Insertar líneas de pedido
                            foreach (var linea in _productosPedido)
                            {
                                command = connection.CreateCommand(
                                    "INSERT INTO LineasPedido (IdPedido, IdProducto, Cantidad, PrecioUnitario) " +
                                    "VALUES (@idPedido, @idProducto, @cantidad, @precio)");

                                command.Parameters.AddWithValue("@idPedido", idPedido);
                                command.Parameters.AddWithValue("@idProducto", linea.IdProducto);
                                command.Parameters.AddWithValue("@cantidad", linea.Cantidad);
                                command.Parameters.AddWithValue("@precio", linea.PrecioUnitario);

                                command.ExecuteNonQuery();

                                // Actualizar stock
                                ProductoDAO.ActualizarStock(linea.IdProducto, linea.Cantidad);
                            }

                            transaction.Commit();

                            MessageBox.Show($"Pedido confirmado. Número de pedido: {idPedido}\nTotal a pagar: {total:C2}",
                                "Confirmado", MessageBoxButton.OK, MessageBoxImage.Information);

                            _productosPedido.Clear();
                            CargarProductosDisponibles();
                            ActualizarTotal();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error de base de datos al confirmar pedido: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al confirmar pedido: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ActualizarTotal()
        {
            txtTotal.Text = _productosPedido.Sum(p => p.Subtotal).ToString("0.00");
        }
    }
}
