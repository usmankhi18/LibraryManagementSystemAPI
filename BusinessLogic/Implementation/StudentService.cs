using BusinessLogic.Interfaces;
using IRepository;
using POCO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Implementation
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public bool DeleteStudent(Student student)
        {
            return studentRepository.DeleteStudent(student);
        }

        public List<Student> GetAllStudents()
        {
            // Call the repository method to get returning students
            return studentRepository.GetAllStudents();
        }

        public List<Student> GetStudents(Student student)
        {
            return studentRepository.GetStudents(student);
        }

        public bool InsertStudents(Student student)
        {
            return studentRepository.InsertStudents(student);
        }

        public bool UpdateStudent(Student student)
        {
            return studentRepository.UpdateStudent(student);
        }
    }
}
