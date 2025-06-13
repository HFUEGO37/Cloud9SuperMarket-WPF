using Cloud9SuperMarket_WPF.Cloud9SuperMarket.Models;

namespace Cloud9SuperMarket_WPF.Cloud9SuperMarket.DataAccess
{
    public class ClienteDAO
    {
        public static Cliente BuscarPorCredenciales(string usuario, string contrasena)
        {
            using (var connection = new DatabaseConnection())
            {
                var command = connection.CreateCommand(
                    "SELECT c.IdCliente, p.* FROM Clientes c " +
                    "JOIN Personas p ON c.IdPersona = p.IdPersona " +
                    "WHERE p.Correo = @usuario AND p.Telefono = @contrasena");

                command.Parameters.AddWithValue("@usuario", usuario);
                command.Parameters.AddWithValue("@contrasena", contrasena);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var persona = new Persona(
                            reader.GetString("Nombre"),
                            reader.GetString("Apellido"),
                            reader.GetString("Dni"),
                            reader.GetString("Correo"),
                            reader.GetString("Telefono")
                        );

                        return new Cliente(
                            reader.GetInt32("IdCliente"),
                            persona
                        );
                    }
                }
            }

            return null;
        }

        public static int RegistrarCliente(Persona persona)
        {
            using (var connection = new DatabaseConnection())
            {
                // Insertar persona
                var command = connection.CreateCommand(
                    "INSERT INTO Personas (Nombre, Apellido, Dni, Correo, Telefono) " +
                    "OUTPUT INSERTED.IdPersona " +
                    "VALUES (@nombre, @apellido, @dni, @correo, @telefono)");

                command.Parameters.AddWithValue("@nombre", persona.Nombre);
                command.Parameters.AddWithValue("@apellido", persona.Apellido);
                command.Parameters.AddWithValue("@dni", persona.Dni);
                command.Parameters.AddWithValue("@correo", persona.Correo);
                command.Parameters.AddWithValue("@telefono", persona.Telefono);

                int idPersona = (int)command.ExecuteScalar();

                // Insertar cliente
                command = connection.CreateCommand(
                    "INSERT INTO Clientes (IdPersona) OUTPUT INSERTED.IdCliente VALUES (@idPersona)");
                command.Parameters.AddWithValue("@idPersona", idPersona);

                return (int)command.ExecuteScalar();
            }
        }
    }
}
