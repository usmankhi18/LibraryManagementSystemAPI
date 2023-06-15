using BusinessLogic.Interfaces;
using Common.Constants;
using Common.FileLogger;
using Common.GenericResponse;
using Microsoft.AspNetCore.Mvc;
using POCO.Models;
using POCO.ResponseDTO;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessModel<List<Student>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseModel))]
        public IActionResult GetAllStudents()
        {
            const string controllerMethod = "StudentController | GetAllStudents";
            try
            {
                var students = service.GetAllStudents();
                //var response = new SingleModelResponse<List<Student>>(false,ResponseCode.Success, ResponseMessage.Success, students);
                SingleModelResponse<List<Student>> response = new SingleModelResponse<List<Student>>();
                response.IsError = false;
                SuccessModel<List<Student>> successModel = new SuccessModel<List<Student>>();
                successModel.ResponseCode = ResponseCode.Success;
                successModel.ResponseDesc = ResponseMessage.Success;
                successModel.ResponseObj = students;
                response.Model = successModel;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Nlog.Error("Exception : " + ex.ToString() + " Stack Trace : " + ex.StackTrace);
                ErrorResponseModel errorModel = new ErrorResponseModel
                {
                    ErrorCode = "9999",
                    Description = "Operation failed to execute.",
                    ErrorCategory = ResponseCategory.InternalServer
                };
                return StatusCode((int)ResponseCategory.GetHttpResp(errorModel.ErrorCategory), errorModel);
            }
        }

        [HttpPost]
        [Route("GetStudents")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessModel<List<Student>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseModel))]
        public IActionResult GetStudents(Student s)
        {
            try
            {
                var students = service.GetStudents(s);
                //var response = new SingleModelResponse<List<Student>>(false,ResponseCode.Success, ResponseMessage.Success, students);
                SingleModelResponse<List<Student>> response = new SingleModelResponse<List<Student>>();
                response.IsError = false;
                SuccessModel<List<Student>> successModel = new SuccessModel<List<Student>>();
                successModel.ResponseCode = ResponseCode.Success;
                successModel.ResponseDesc = ResponseMessage.Success;
                successModel.ResponseObj = students;
                response.Model = successModel;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Nlog.Error("Exception : " + ex.ToString() + " Stack Trace : " + ex.StackTrace);
                ErrorResponseModel errorModel = new ErrorResponseModel
                {
                    ErrorCode = "9999",
                    Description = "Operation failed to execute.",
                    ErrorCategory = ResponseCategory.InternalServer
                };
                return StatusCode((int)ResponseCategory.GetHttpResp(errorModel.ErrorCategory), errorModel);
            }
        }

        [HttpPost]
        [Route("InsertStudent")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessModel<InsertDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseModel))]
        public IActionResult InsertStudent(Student s)
        {
            try
            {
                var students = service.InsertStudents(s);
                SingleModelResponse<InsertDTO> response = new SingleModelResponse<InsertDTO>();
                response.IsError = false;
                SuccessModel<InsertDTO> successModel = new SuccessModel<InsertDTO>();
                successModel.ResponseCode = ResponseCode.Success;
                successModel.ResponseDesc = ResponseMessage.Success;
                successModel.ResponseObj = students;
                response.Model = successModel;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Nlog.Error("Exception : " + ex.ToString() + " Stack Trace : " + ex.StackTrace);
                ErrorResponseModel errorModel = new ErrorResponseModel
                {
                    ErrorCode = "9999",
                    Description = "Operation failed to execute.",
                    ErrorCategory = ResponseCategory.InternalServer
                };
                return StatusCode((int)ResponseCategory.GetHttpResp(errorModel.ErrorCategory), errorModel);
            }
        }

        [HttpPut]
        [Route("UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessModel<UpdateDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseModel))]
        public IActionResult UpdateStudent(Student s)
        {
            try
            {
                var students = service.UpdateStudent(s);
                SingleModelResponse<UpdateDTO> response = new SingleModelResponse<UpdateDTO>();
                response.IsError = false;
                SuccessModel<UpdateDTO> successModel = new SuccessModel<UpdateDTO>();
                successModel.ResponseCode = ResponseCode.Success;
                successModel.ResponseDesc = ResponseMessage.Success;
                successModel.ResponseObj = students;
                response.Model = successModel;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Nlog.Error("Exception : " + ex.ToString() + " Stack Trace : " + ex.StackTrace);
                ErrorResponseModel errorModel = new ErrorResponseModel
                {
                    ErrorCode = "9999",
                    Description = "Operation failed to execute.",
                    ErrorCategory = ResponseCategory.InternalServer
                };
                return StatusCode((int)ResponseCategory.GetHttpResp(errorModel.ErrorCategory), errorModel);
            }
        }

        [HttpDelete]
        [Route("DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessModel<DeleteDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseModel))]
        public IActionResult DeleteStudent(Student s)
        {
            try
            {
                var students = service.DeleteStudent(s);
                SingleModelResponse<DeleteDTO> response = new SingleModelResponse<DeleteDTO>();
                response.IsError = false;
                SuccessModel<DeleteDTO> successModel = new SuccessModel<DeleteDTO>();
                successModel.ResponseCode = ResponseCode.Success;
                successModel.ResponseDesc = ResponseMessage.Success;
                successModel.ResponseObj = students;
                response.Model = successModel;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Nlog.Error("Exception : " + ex.ToString() + " Stack Trace : " + ex.StackTrace);
                ErrorResponseModel errorModel = new ErrorResponseModel
                {
                    ErrorCode = "9999",
                    Description = "Operation failed to execute.",
                    ErrorCategory = ResponseCategory.InternalServer
                };
                return StatusCode((int)ResponseCategory.GetHttpResp(errorModel.ErrorCategory), errorModel);
            }
        }
    }
}
