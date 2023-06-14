using BusinessLogic.Interfaces;
using Common.Constants;
using Common.GenericResponse;
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
            try
            {
                var students = service.GetAllStudents();
                var response = new ApiResponse<List<Student>>(false,ResponseCode.Success, ResponseMessage.Success, students);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                Console.WriteLine($"An error occurred while retrieving all students: {ex.Message}");

                var response = new ApiResponse<string>(true,ResponseCode.Error, ResponseMessage.Error, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("GetStudents")]
        public IActionResult GetStudents(Student s)
        {
            try
            {
                var students = service.GetStudents(s);
                var response = new ApiResponse<List<Student>>(false, ResponseCode.Success, ResponseMessage.Success, students);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                Console.WriteLine($"An error occurred while retrieving students: {ex.Message}");

                var response = new ApiResponse<string>(true, ResponseCode.Error, ResponseMessage.Error, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("InsertStudent")]
        public IActionResult InsertStudent(Student s)
        {
            try
            {
                var students = service.InsertStudents(s);
                var response = new ApiResponse<bool>(false, ResponseCode.Success, ResponseMessage.Success, students);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                Console.WriteLine($"An error occurred while inserting students: {ex.Message}");

                var response = new ApiResponse<string>(true, ResponseCode.Error, ResponseMessage.Error, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut]
        [Route("UpdateStudent")]
        public IActionResult UpdateStudent(Student s)
        {
            try
            {
                var students = service.UpdateStudent(s);
                var response = new ApiResponse<bool>(false, ResponseCode.Success, ResponseMessage.Success, students);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                Console.WriteLine($"An error occurred while updating students: {ex.Message}");

                var response = new ApiResponse<string>(true, ResponseCode.Error, ResponseMessage.Error, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpDelete]
        [Route("DeleteStudent")]
        public IActionResult DeleteStudent(Student s)
        {
            try
            {
                var students = service.DeleteStudent(s);
                var response = new ApiResponse<bool>(false, ResponseCode.Success, ResponseMessage.Success, students);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                Console.WriteLine($"An error occurred while deleting students: {ex.Message}");

                var response = new ApiResponse<string>(true, ResponseCode.Error, ResponseMessage.Error, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
