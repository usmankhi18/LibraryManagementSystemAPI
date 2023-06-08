using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServer.DBContext
{
    public interface IDatabaseContext
    {
        void Open();
        void Close();
        SqlDataReader ExecuteReader(SqlCommand command);
        int ExecuteNonQuery(SqlCommand command);
    }

    public class SqlConnectionContext : IDatabaseContext
    {
        private readonly SqlConnection connection;

        public SqlConnectionContext(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }

        public void Open()
        {
            connection.Open();
        }

        public void Close()
        {
            connection.Close();
        }

        public SqlDataReader ExecuteReader(SqlCommand command)
        {
            command.Connection = connection;
            return command.ExecuteReader();
        }

        public int ExecuteNonQuery(SqlCommand command)
        {
            command.Connection = connection;
            return command.ExecuteNonQuery();
        }
    }

}
