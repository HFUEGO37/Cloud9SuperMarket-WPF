namespace Cloud9SuperMarket_WPF.Cloud9SuperMarket.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set; }

        public Producto() { }

        public Producto(int id, string nombre, string categoria, double precio, int stock)
        {
            IdProducto = id;
            Nombre = nombre;
            Categoria = categoria;
            Precio = precio;
            Stock = stock;
        }
    }
}

