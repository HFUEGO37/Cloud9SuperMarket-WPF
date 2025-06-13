using System;
using System.Collections.Generic;

namespace Cloud9SuperMarket_WPF.Cloud9SuperMarket.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public List<LineaPedido> Productos { get; set; }
        public DateTime Fecha { get; set; }
        public double Total { get; set; }

        public Pedido()
        {
            Productos = new List<LineaPedido>();
            Fecha = DateTime.Now;
        }

        public Pedido(int idPedido, int idCliente, List<LineaPedido> productos, DateTime fecha, double total)
        {
            IdPedido = idPedido;
            IdCliente = idCliente;
            Productos = productos ?? new List<LineaPedido>();
            Fecha = fecha;
            Total = total;
        }

        private double CalcularTotal()
        {
            double total = 0;
            foreach (var linea in Productos)
            {
                total += linea.Subtotal;
            }
            return total;
        }
    }
}