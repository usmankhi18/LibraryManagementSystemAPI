using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posgres.DBContext
{
    public interface IDatabaseContext
    {
        void Open();
        void Close();
        NpgsqlDataReader ExecuteReader(NpgsqlCommand command);
        object ExecuteScalar(NpgsqlCommand command);
        NpgsqlCommand CreateCommand();
    }
    public class PosgresConnectionContext : IDatabaseContext
    {
        private readonly NpgsqlConnection connection;

        public PosgresConnectionContext(string connectionString)
        {
            connection = new NpgsqlConnection(connectionString);
        }

        public void Open()
        {
            connection.Open();
        }

        public void Close()
        {
            connection.Close();
        }

        public NpgsqlDataReader ExecuteReader(NpgsqlCommand command)
        {
            command.Connection = connection;
            return command.ExecuteReader();
        }

        public object ExecuteScalar(NpgsqlCommand command)
        {
            command.Connection = connection;
            return command.ExecuteScalar();
        }

        public NpgsqlCommand CreateCommand()
        {
            return connection.CreateCommand();
        }
    }
}
