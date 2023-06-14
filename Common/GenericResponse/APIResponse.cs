using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.GenericResponse
{
    public class ApiResponse<T>
    {
        public bool IsError { get; set; }
        public string ResponseCode { get; init; }
        public string ResponseMessage { get; init; }
        public T ResponseData { get; init; }

        public ApiResponse(bool IsError,string code, string message, T data)
        {
            this.IsError = IsError;
            ResponseCode = code;
            ResponseMessage = message;
            ResponseData = data;
        }
    }
}
