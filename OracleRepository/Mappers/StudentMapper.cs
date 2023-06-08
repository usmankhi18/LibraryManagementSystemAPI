using Oracle.ManagedDataAccess.Client;
using POCO.Models;
using System.Data;

namespace OracleRepository.Mappers
{
    public class StudentMapper
    {
        public static Student MapFromReader(OracleDataReader reader)
        {
            Student student = new Student();
            student.StudentId = Convert.ToInt32(reader["StudentId"]);
            student.FirstName = reader["FirstName"].ToString();
            student.LastName = reader["LastName"].ToString();
            return student;
        }

        public static void SetInsertParameters(OracleCommand command, Student student)
        {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "StudentPackage.INSERT_STUDENT";
            command.Parameters.Add("p_FirstName", OracleDbType.Varchar2).Value = student.FirstName;
            command.Parameters.Add("p_LastName", OracleDbType.Varchar2).Value = student.LastName;
            command.Parameters.Add("p_Inserted", OracleDbType.Int32).Direction = ParameterDirection.Output;
        }

        public static void SetDeleteParameters(OracleCommand command, Student student)
        {
            command.CommandText = "StudentPackage.DELETE_STUDENT";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("p_StudentId", OracleDbType.Int32).Value = student.StudentId;
            command.Parameters.Add("p_IsDeleted", OracleDbType.Int32).Direction = ParameterDirection.Output;
        }

        public static void SetUpdateParameters(OracleCommand command, Student student)
        {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "StudentPackage.UPDATE_STUDENT";
            command.Parameters.Add("p_StudentId", OracleDbType.Int32).Value = student.StudentId;
            command.Parameters.Add("p_FirstName", OracleDbType.Varchar2).Value = student.FirstName;
            command.Parameters.Add("p_LastName", OracleDbType.Varchar2).Value = student.LastName;
            command.Parameters.Add("p_IsUpdated", OracleDbType.Int32).Direction = ParameterDirection.Output;
        }

        public static void SetGetStudentsParameters(OracleCommand command, Student student)
        {
            command.CommandText = "StudentPackage.GET_STUDENTS";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("p_FirstNameFilter", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(student.FirstName) ? (object)DBNull.Value : student.FirstName;
            command.Parameters.Add("p_LastNameFilter", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(student.LastName) ? (object)DBNull.Value : student.LastName;
            command.Parameters.Add("p_StudentId", OracleDbType.Int32).Value = student.StudentId;
            command.Parameters.Add("p_Students", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
        }
    }
}
