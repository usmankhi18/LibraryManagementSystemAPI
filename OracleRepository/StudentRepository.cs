using IRepository;
using POCO.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.DBContext;
using Oracle.ManagedDataAccess.Types;
using OracleRepository.Mappers;

namespace Oracle
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
                using (OracleCommand command = databaseContext.CreateCommand())
                {
                    StudentMapper.SetDeleteParameters(command, student);
                    databaseContext.ExecuteNonQuery(command);
                    int isDeleted = Convert.ToInt32(((OracleDecimal)command.Parameters["p_IsDeleted"].Value).ToInt32());
                    return isDeleted == 1; // Return true if insert was successful
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

                using (OracleCommand command = databaseContext.CreateCommand())
                {
                    command.CommandText = "StudentPackage.GET_ALL_STUDENTS";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Create the OUT parameter for the result set
                    OracleParameter studentsCursor = new OracleParameter();
                    studentsCursor.ParameterName = "p_Students";
                    studentsCursor.OracleDbType = OracleDbType.RefCursor;
                    studentsCursor.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(studentsCursor);

                    // Execute the stored procedure
                    using (OracleDataReader reader = databaseContext.ExecuteReader(command))
                    {
                        while (reader.Read())
                        {
                            Student student = StudentMapper.MapFromReader(reader);
                            students.Add(student);
                        }
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

                using (OracleCommand command = databaseContext.CreateCommand())
                {

                    StudentMapper.SetGetStudentsParameters(command, student);

                    using (OracleDataReader reader = databaseContext.ExecuteReader(command))
                    {
                        while (reader.Read())
                        {
                            Student s = StudentMapper.MapFromReader(reader);
                            students.Add(s);
                        }
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

                using (OracleCommand command = databaseContext.CreateCommand())
                {
                    StudentMapper.SetInsertParameters(command, student);

                databaseContext.ExecuteNonQuery(command);

                    int isInserted = Convert.ToInt32(((OracleDecimal)command.Parameters["p_Inserted"].Value).ToInt32());

                    return isInserted == 1; // Return true if insert was successful
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

                using (OracleCommand command = databaseContext.CreateCommand())
                {
                    StudentMapper.SetUpdateParameters(command, student);

                    databaseContext.ExecuteNonQuery(command);

                    int isUpdated = Convert.ToInt32(((OracleDecimal)command.Parameters["p_IsUpdated"].Value).ToInt32());

                    return isUpdated == 1; // Return true if insert was successful
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
