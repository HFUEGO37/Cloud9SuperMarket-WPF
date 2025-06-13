namespace Cloud9SuperMarket_WPF.Cloud9SuperMarket.Models
{
    public class Persona
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }

        public Persona() { }

        public Persona(string nombre, string apellido, string dni, string correo, string telefono)
        {
            Nombre = nombre;
            Apellido = apellido;
            Dni = dni;
            Correo = correo;
            Telefono = telefono;
        }
    }
}



