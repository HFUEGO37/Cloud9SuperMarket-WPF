using Cloud9SuperMarket_WPF.Cloud9SuperMarket.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Cloud9SuperMarket_WPF.Cloud9SuperMarket.DataAccess
{
    public class ProductoDAO
    {
        public static List<Producto> ObtenerTodos()
        {
            var productos = new List<Producto>();

            using (var connection = new DatabaseConnection())
            {
                var command = connection.CreateCommand("SELECT * FROM Productos");

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productos.Add(new Producto(
                            reader.GetInt32("IdProducto"),
                            reader.GetString("Nombre"),
                            reader.GetString("Categoria"),
                            reader.GetDouble("Precio"),
                            reader.GetInt32("Stock")
                        ));
                    }
                }
            }

            return productos;
        }

        public static void ActualizarStock(int idProducto, int cantidad)
        {
            using (var connection = new DatabaseConnection())
            {
                var command = connection.CreateCommand(
                    "UPDATE Productos SET Stock = Stock - @cantidad WHERE IdProducto = @id");

                command.Parameters.AddWithValue("@cantidad", cantidad);
                command.Parameters.AddWithValue("@id", idProducto);

                command.ExecuteNonQuery();
            }
        }

        // Método adicional para insertar un nuevo producto
        public static int InsertarProducto(Producto producto)
        {
            using (var connection = new DatabaseConnection())
            {
                var command = connection.CreateCommand(
                    "INSERT INTO Productos (Nombre, Categoria, Precio, Stock) " +
                    "VALUES (@nombre, @categoria, @precio, @stock); " +
                    "SELECT LAST_INSERT_ID();");

                command.Parameters.AddWithValue("@nombre", producto.Nombre);
                command.Parameters.AddWithValue("@categoria", producto.Categoria);
                command.Parameters.AddWithValue("@precio", producto.Precio);
                command.Parameters.AddWithValue("@stock", producto.Stock);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
}