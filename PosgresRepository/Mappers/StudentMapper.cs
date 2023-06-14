using Npgsql;
using POCO.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posgres.Mappers
{
    public class StudentMapper
    {
        public static List<Student> MapFromReader(NpgsqlDataReader reader)
        {
            List<Student> students = new List<Student>();
            while (reader.Read())
            {
                Student student = new Student
                {
                    StudentId = reader.GetInt32(reader.GetOrdinal("studentid")),
                    FirstName = reader.GetString(reader.GetOrdinal("firstname")),
                    LastName = reader.GetString(reader.GetOrdinal("lastname"))
                };

                students.Add(student);
            }
            return students;
        }

        public static void SetInsertParameters(NpgsqlCommand command, Student student)
        {
            command.CommandText = "SELECT public.insert_student(@FirstName, @LastName)";
            command.Parameters.AddWithValue("@FirstName", student.FirstName);
            command.Parameters.AddWithValue("@LastName", student.LastName);
        }

        public static void SetDeleteParameters(NpgsqlCommand command, Student student)
        {
            command.CommandText = "SELECT public.delete_student(@StudentId)";
            command.Parameters.AddWithValue("@StudentId", student.StudentId);
        }

        public static void SetUpdateParameters(NpgsqlCommand command, Student student)
        {
            command.CommandText = "SELECT public.update_student(@StudentId, @FirstName, @LastName)";
            command.Parameters.AddWithValue("@StudentId", student.StudentId);
            command.Parameters.AddWithValue("@FirstName", student.FirstName);
            command.Parameters.AddWithValue("@LastName", student.LastName);
        }

        public static void SetGetStudentsParameters(NpgsqlCommand command, Student student)
        {
            command.CommandText = "SELECT * FROM public.get_students(@FirstName, @LastName, @StudentId)";
            command.Parameters.AddWithValue("@FirstName", string.IsNullOrEmpty(student.FirstName) ? (object)DBNull.Value : student.FirstName);
            command.Parameters.AddWithValue("@LastName", string.IsNullOrEmpty(student.LastName) ? (object)DBNull.Value : student.LastName);
            command.Parameters.AddWithValue("@StudentId", student.StudentId > 0 ? (object)student.StudentId : (object)DBNull.Value);
        }
    }
}
