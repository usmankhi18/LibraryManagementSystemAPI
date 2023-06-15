using POCO.Models;
using POCO.ResponseDTO;

namespace BusinessLogic.Interfaces
{
    public interface IStudentService
    {
        public List<Student> GetAllStudents();
        public InsertDTO InsertStudents(Student student);
        public List<Student> GetStudents(Student student);
        public UpdateDTO UpdateStudent(Student student);
        public DeleteDTO DeleteStudent(Student student);
    }
}
