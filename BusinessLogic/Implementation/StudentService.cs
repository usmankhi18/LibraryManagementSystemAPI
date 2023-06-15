using BusinessLogic.ApplicationCache;
using BusinessLogic.Interfaces;
using Global.AppSettings;
using IRepository;
using POCO.Models;
using POCO.ResponseDTO;

namespace BusinessLogic.Implementation
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;
        private readonly StudentCache studentCache;

        public StudentService(IStudentRepository studentRepository,StudentCache cache)
        {
            this.studentRepository = studentRepository;
            this.studentCache = cache;
        }

        public DeleteDTO DeleteStudent(Student student)
        {
            DeleteDTO deleteDTO = new DeleteDTO();
            deleteDTO.IsDeleted = studentRepository.DeleteStudent(student);
            studentCache.ClearCache(AppSettingKeys.RedisKey);
            return deleteDTO;
        }

        public List<Student> GetAllStudents()
        {
            List<Student> students = studentCache.GetStudentsFromCache(AppSettingKeys.RedisKey);
            if (students == null)
            {
                return studentRepository.GetAllStudents();
            }
            return students;
        }

        public List<Student> GetStudents(Student student)
        {
            return studentRepository.GetStudents(student);
        }

        public InsertDTO InsertStudents(Student student)
        {
            InsertDTO insertDTO = new InsertDTO();
            insertDTO.IsInserted = studentRepository.InsertStudents(student);
            studentCache.ClearCache(AppSettingKeys.RedisKey);
            return insertDTO;
        }

        public UpdateDTO UpdateStudent(Student student)
        {
            UpdateDTO updateDTO = new UpdateDTO();
            updateDTO.IsUpdated = studentRepository.UpdateStudent(student);
            studentCache.ClearCache(AppSettingKeys.RedisKey);
            return updateDTO;
        }
    }
}
