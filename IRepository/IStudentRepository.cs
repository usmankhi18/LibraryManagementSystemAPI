using POCO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IStudentRepository
    {
        public List<Student> GetAllStudents();
        public bool InsertStudents(Student student);
        public List<Student> GetStudents(Student student);
        public bool UpdateStudent(Student student);
        public bool DeleteStudent(Student student);
    }
}
