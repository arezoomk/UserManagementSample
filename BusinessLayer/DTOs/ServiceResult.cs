using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class ServiceResult<T>
    { 
        public T? Data { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        public static ServiceResult<T> SuccessResult(T _result, string message = "Operation succeeded.")
        {
            return new ServiceResult<T>(_result, message);
        }

        protected ServiceResult(T _result, string message)
        {
            Success = true;
            Message = message;
            Data = _result;
        }

        public static ServiceResult<T> FailureResult(string message)
        {
            return new ServiceResult<T>(message);
        }

        protected ServiceResult(string message)
        {
            Success = false;
            Message = message;
        }

    }
    
}
