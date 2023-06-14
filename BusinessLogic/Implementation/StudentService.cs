using BusinessLogic.ApplicationCache;
using BusinessLogic.Interfaces;
using Global.AppSettings;
using IRepository;
using POCO.Models;
using StackExchange.Redis;
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
        private readonly StudentCache studentCache;

        public StudentService(IStudentRepository studentRepository,StudentCache cache)
        {
            this.studentRepository = studentRepository;
            this.studentCache = cache;
        }

        public bool DeleteStudent(Student student)
        {
            bool isDelete = studentRepository.DeleteStudent(student);
            studentCache.ClearCache(AppSettingKeys.RedisKey);
            return isDelete;
        }

        public List<Student> GetAllStudents()
        {
            List<Student> students = studentCache.GetStudentsFromCache(AppSettingKeys.RedisKey);
            if (students == null)
            {
                // Call the repository method to get returning students
                return studentRepository.GetAllStudents();
            }
            return students;
        }

        public List<Student> GetStudents(Student student)
        {
            return studentRepository.GetStudents(student);
        }

        public bool InsertStudents(Student student)
        {
            bool isInsert = studentRepository.InsertStudents(student);
            studentCache.ClearCache(AppSettingKeys.RedisKey);
            return isInsert;
        }

        public bool UpdateStudent(Student student)
        {
            bool isUpdate = studentRepository.UpdateStudent(student);
            studentCache.ClearCache(AppSettingKeys.RedisKey);
            return isUpdate;
        }
    }
}
