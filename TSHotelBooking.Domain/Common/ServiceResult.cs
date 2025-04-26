using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSHotelBooking.Domain.Common
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T Data { get; set; }

        private ServiceResult(bool success, string message, int statusCode, T data)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
            Data = data;
        }

        public static ServiceResult<T> Ok(T data, string message = "Success")
        {
            return new ServiceResult<T>(true, message, 200, data);
        }

        public static ServiceResult<T> Created(T data, string message = "Created")
        {
            return new ServiceResult<T>(true, message, 201, data);
        }

        public static ServiceResult<T> Failure(string message, int statusCode = 400)
        {
            return new ServiceResult<T>(false, message, statusCode, default);
        }
    }
}
