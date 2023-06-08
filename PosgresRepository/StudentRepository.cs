using IRepository;
using Npgsql;
using POCO.Models;
using System.Data;

namespace Posgres
{
    public class StudentRepository : IStudentRepository
    {
        public bool DeleteStudent(Student student)
        {
            throw new NotImplementedException();
        }

        public List<Student> GetAllStudents()
        {
            throw new NotImplementedException();
        }

        public List<Student> GetStudents(Student student)
        {
            throw new NotImplementedException();
        }

        public bool InsertStudents(Student student)
        {
            throw new NotImplementedException();
        }

        public bool UpdateStudent(Student student)
        {
            throw new NotImplementedException();
        }
    }
}