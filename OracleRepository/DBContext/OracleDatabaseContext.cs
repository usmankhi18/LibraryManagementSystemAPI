using Oracle.ManagedDataAccess.Client;
using System;

namespace Oracle.DBContext
{
    public interface IDatabaseContext : IDisposable
    {
        void Open();
        void Close();
        OracleDataReader ExecuteReader(OracleCommand command);
        int ExecuteNonQuery(OracleCommand command);
        OracleCommand CreateCommand();
    }

    public class OracleDatabaseContext : IDatabaseContext
    {
        private readonly OracleConnection connection;

        public OracleDatabaseContext(string connectionString)
        {
            connection = new OracleConnection(connectionString);
        }

        public void Open()
        {
            connection.Open();
        }

        public void Close()
        {
            connection.Close();
        }

        public OracleDataReader ExecuteReader(OracleCommand command)
        {
            command.Connection = connection;
            return command.ExecuteReader();
        }

        public int ExecuteNonQuery(OracleCommand command)
        {
            command.Connection = connection;
            return command.ExecuteNonQuery();
        }

        public OracleCommand CreateCommand()
        {
            return connection.CreateCommand();
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
