namespace Cloud9SuperMarket_WPF.Cloud9SuperMarket.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public Persona Persona { get; set; }

        public Cliente() { }

        public Cliente(int id, Persona persona)
        {
            IdCliente = id;
            Persona = persona;
        }

        public override string ToString()
        {
            return $"{Persona.Nombre} {Persona.Apellido}";
        }
    }
}


