using MySql.Data.MySqlClient;
using System;
using System.Configuration;

namespace Cloud9SuperMarket_WPF.Cloud9SuperMarket.DataAccess
{
    public class DatabaseConnection : IDisposable
    {
        private readonly MySqlConnection _connection;
        private MySqlTransaction _transaction;

        public DatabaseConnection()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings?["Cloud9SupermarketDB"]?.ConnectionString;

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new ConfigurationErrorsException(
                        "Cadena de conexión 'Cloud9SupermarketDB' no encontrada o vacía en App.config");
                }

                _connection = new MySqlConnection(connectionString);
                _connection.Open();
            }
            catch (ConfigurationErrorsException)
            {
                throw; // Relanzamos para manejar específicamente
            }
            catch (Exception ex)
            {
                throw new Exception("Error al conectar a la base de datos", ex);
            }
        }

        public MySqlCommand CreateCommand(string query)
        {
            var command = new MySqlCommand(query, _connection);
            if (_transaction != null)
            {
                command.Transaction = _transaction;
            }
            return command;
        }

        public MySqlTransaction BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
            return _transaction;
        }

        public void Commit()
        {
            _transaction?.Commit();
            _transaction = null;
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction = null;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}