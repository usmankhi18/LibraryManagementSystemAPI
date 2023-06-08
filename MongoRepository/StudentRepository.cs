using Global.AppSettings;
using IRepository;
using MongoDB.Bson;
using MongoDB.Driver;
using POCO.Models;

namespace Mongo
{
    public class StudentRepository : IStudentRepository
    {
        private IMongoCollection<Student> collection;

        public StudentRepository(IMongoCollection<Student> mongoCollection)
        {
            this.collection = mongoCollection;
        }

        public bool DeleteStudent(Student student)
        {
            var filter = Builders<Student>.Filter.Eq(s => s.StudentId, student.StudentId);
            var result = collection.DeleteMany(filter);
            return result.DeletedCount > 0;
        }

        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();

            var filter = Builders<Student>.Filter.Empty;
            var studentsResp = collection.Find(filter).ToList();

            students.AddRange(studentsResp);

            return students;
        }

        public List<Student> GetStudents(Student student)
        {
            var filterBuilder = Builders<Student>.Filter;
            var filterList = new List<FilterDefinition<Student>>();

            if (!string.IsNullOrEmpty(student.FirstName))
            {
                filterList.Add(filterBuilder.Eq(s => s.FirstName, student.FirstName));
            }

            if (!string.IsNullOrEmpty(student.LastName))
            {
                filterList.Add(filterBuilder.Eq(s => s.LastName, student.LastName));
            }

            if (student.StudentId != 0)
            {
                filterList.Add(filterBuilder.Eq(s => s.StudentId, student.StudentId));
            }

            var filter = filterBuilder.And(filterList);
            var studentsResp = collection.Find(filter).ToList();
            return studentsResp;
        }

        public bool InsertStudents(Student student)
        {
            // Get the maximum existing student ID
            var maxId = collection.AsQueryable().Max(s => s.StudentId);

            // Set the new student ID as the maximum ID + 1
            student.StudentId = maxId + 1;
            collection.InsertOne(student);
            return true;
        }

        public bool UpdateStudent(Student student)
        {
            var filter = Builders<Student>.Filter.Eq(s => s.StudentId, student.StudentId);
            var update = Builders<Student>.Update
                .Set(s => s.FirstName, student.FirstName)
                .Set(s => s.LastName, student.LastName);
            var result = collection.UpdateMany(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}