namespace Cloud9SuperMarket_WPF.Cloud9SuperMarket.Models
{
    public class LineaPedido
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
        public double Subtotal => Cantidad * PrecioUnitario;
    }
}

