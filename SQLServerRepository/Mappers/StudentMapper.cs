using POCO.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServer.Mappers
{
    public class StudentMapper
    {
        public static Student MapFromReader(SqlDataReader reader)
        {
            Student student = new Student();
            student.StudentId = Convert.ToInt32(reader["StudentId"]);
            student.FirstName = reader["FirstName"].ToString();
            student.LastName = reader["LastName"].ToString();
            return student;
        }

        public static void SetInsertParameters(SqlCommand command, Student student)
        {
            command.Parameters.AddWithValue("@FirstName", student.FirstName);
            command.Parameters.AddWithValue("@LastName", student.LastName);
        }

        public static void SetDeleteParameters(SqlCommand command, Student student)
        {
            command.Parameters.AddWithValue("@StudentId", student.StudentId);
        }

        public static void SetUpdateParameters(SqlCommand command, Student student)
        {
            command.Parameters.AddWithValue("@StudentId", student.StudentId);
            command.Parameters.AddWithValue("@FirstName", student.FirstName);
            command.Parameters.AddWithValue("@LastName", student.LastName);
        }
    }
}
