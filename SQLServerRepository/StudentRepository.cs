using Global.AppSettings;
using IRepository;
using POCO.Models;
using SQLServer.DBContext;
using SQLServer.Mappers;
using System.Data;
using System.Data.SqlClient;

namespace SQLServer
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IDatabaseContext databaseContext;

        public StudentRepository(IDatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public List<Student> GetAllStudents()
        {
            List<Student> returningStudents = new List<Student>();

            databaseContext.Open();

            using (SqlCommand command = new SqlCommand("GetAllStudents_SP"))
            {
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = databaseContext.ExecuteReader(command);

                while (reader.Read())
                {
                    Student student = StudentMapper.MapFromReader(reader);
                    returningStudents.Add(student);
                }

                reader.Close();
            }

            databaseContext.Close();

            return returningStudents;
        }

        public List<Student> GetStudents(Student student)
        {
            List<Student> filteredStudents = new List<Student>();

            databaseContext.Open();

            using (SqlCommand command = new SqlCommand("GetStudentsByFilter_SP"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FirstNameFilter", string.IsNullOrEmpty(student.FirstName) ? (object)DBNull.Value : student.FirstName);
                command.Parameters.AddWithValue("@LastNameFilter", string.IsNullOrEmpty(student.LastName) ? (object)DBNull.Value : student.LastName);
                command.Parameters.AddWithValue("@ID", student.StudentId);

                SqlDataReader reader = databaseContext.ExecuteReader(command);

                while (reader.Read())
                {
                    Student stud = StudentMapper.MapFromReader(reader);
                    filteredStudents.Add(stud);
                }

                reader.Close();
            }

            databaseContext.Close();

            return filteredStudents;
        }

        public bool InsertStudents(Student student)
        {
            databaseContext.Open();

            using (SqlCommand command = new SqlCommand("CreateStudent_SP"))
            {
                command.CommandType = CommandType.StoredProcedure;
                StudentMapper.SetInsertParameters(command, student);

                int rowsAffected = databaseContext.ExecuteNonQuery(command);

                databaseContext.Close();

                return rowsAffected > 0;
            }
        }

        public bool DeleteStudent(Student student)
        {
            databaseContext.Open();

            using (SqlCommand command = new SqlCommand("DeleteStudent_SP"))
            {
                command.CommandType = CommandType.StoredProcedure;
                StudentMapper.SetDeleteParameters(command, student);

                int rowsAffected = databaseContext.ExecuteNonQuery(command);

                databaseContext.Close();

                return rowsAffected > 0;
            }
        }

        public bool UpdateStudent(Student student)
        {
            databaseContext.Open();

            using (SqlCommand command = new SqlCommand("UpdateStudent_SP"))
            {
                command.CommandType = CommandType.StoredProcedure;
                StudentMapper.SetUpdateParameters(command, student);

                int rowsAffected = databaseContext.ExecuteNonQuery(command);

                databaseContext.Close();

                return rowsAffected > 0;
            }
        }
    }
}