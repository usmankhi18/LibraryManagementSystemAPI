using Global.AppSettings;
using IRepository;
using Npgsql;
using POCO.Models;
using Posgres.DBContext;
using Posgres.Mappers;
using System.Data;

namespace Posgres
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IDatabaseContext databaseContext;
        public StudentRepository(IDatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public bool DeleteStudent(Student student)
        {
            try
            {
                databaseContext.Open();
                using (NpgsqlCommand command = databaseContext.CreateCommand())
                {
                    StudentMapper.SetDeleteParameters(command, student);
                    bool deletionSuccess = (bool)databaseContext.ExecuteScalar(command);
                    return deletionSuccess;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();
            try
            {
                databaseContext.Open();

                using (NpgsqlCommand command = databaseContext.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM public.get_students()";

                    // Execute the stored procedure
                    using (NpgsqlDataReader reader = databaseContext.ExecuteReader(command))
                    {
                        students = StudentMapper.MapFromReader(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                databaseContext.Close();
            }

            return students;
        }

        public List<Student> GetStudents(Student student)
        {
            List<Student> students = new List<Student>();

            try
            {
                databaseContext.Open();

                using (NpgsqlCommand command = databaseContext.CreateCommand())
                {

                    StudentMapper.SetGetStudentsParameters(command, student);

                    using (NpgsqlDataReader reader = databaseContext.ExecuteReader(command))
                    {
                        students = StudentMapper.MapFromReader(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                databaseContext.Close();
            }

            return students;
        }

        public bool InsertStudents(Student student)
        {
            try
            {
                databaseContext.Open();
                using (NpgsqlCommand command = databaseContext.CreateCommand())
                {
                    StudentMapper.SetInsertParameters(command, student);
                    bool insertionSuccess = (bool)databaseContext.ExecuteScalar(command);
                    return insertionSuccess;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateStudent(Student student)
        {
            try
            {
                databaseContext.Open();
                using (NpgsqlCommand command = databaseContext.CreateCommand())
                {
                    StudentMapper.SetUpdateParameters(command, student);
                    bool updationSuccess = (bool)databaseContext.ExecuteScalar(command);
                    return updationSuccess;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}