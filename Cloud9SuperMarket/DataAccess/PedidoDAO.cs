using Cloud9SuperMarket_WPF.Cloud9SuperMarket.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Cloud9SuperMarket_WPF.Cloud9SuperMarket.DataAccess
{
    public class PedidoDAO
    {
        public static int CrearPedido(Pedido pedido)
        {
            using (var connection = new DatabaseConnection())
            {
                // Insertar pedido
                var command = connection.CreateCommand(
                    "INSERT INTO Pedidos (IdCliente, Fecha, Total) " +
                    "VALUES (@idCliente, @fecha, @total); " +
                    "SELECT LAST_INSERT_ID();");

                command.Parameters.AddWithValue("@idCliente", pedido.IdCliente);
                command.Parameters.AddWithValue("@fecha", pedido.Fecha);
                command.Parameters.AddWithValue("@total", pedido.Total);

                int idPedido = Convert.ToInt32(command.ExecuteScalar());

                // Insertar líneas de pedido
                foreach (var linea in pedido.Productos)
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

                return idPedido;
            }
        }

        public static List<Pedido> ObtenerPedidosPorCliente(int idCliente)
        {
            var pedidos = new List<Pedido>();

            using (var connection = new DatabaseConnection())
            {
                var command = connection.CreateCommand(
                    "SELECT p.* FROM Pedidos p WHERE p.IdCliente = @idCliente");
                command.Parameters.AddWithValue("@idCliente", idCliente);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pedidos.Add(new Pedido(
                            reader.GetInt32("IdPedido"),
                            reader.GetInt32("IdCliente"),
                            new List<LineaPedido>(),
                            reader.GetDateTime("Fecha"),
                            reader.GetDouble("Total")
                        ));
                    }
                }
            }

            return pedidos;
        }
    }
}