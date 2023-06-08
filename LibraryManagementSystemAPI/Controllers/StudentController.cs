using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POCO.Models;

namespace LibraryManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService service;

        public StudentController(IStudentService studentService)
        {
            service = studentService;
        }

        [HttpGet]
        [Route("GetAllStudents")]
        public IActionResult GetAllStudents()
        {
            var students = service.GetAllStudents();
            return Ok(students);
        }

        [HttpPost]
        [Route("GetStudents")]
        public IActionResult GetStudents(Student s)
        {
            var students = service.GetStudents(s);
            return Ok(students);
        }

        [HttpPost]
        [Route("InsertStudent")]
        public IActionResult InsertStudent(Student s)
        {
            var students = service.InsertStudents(s);
            return Ok(students);
        }

        [HttpPut]
        [Route("UpdateStudent")]
        public IActionResult UpdateStudent(Student s)
        {
            var students = service.UpdateStudent(s);
            return Ok(students);
        }

        [HttpDelete]
        [Route("DeleteStudent")]
        public IActionResult DeleteStudent(Student s)
        {
            var students = service.DeleteStudent(s);
            return Ok(students);
        }
    }
}
