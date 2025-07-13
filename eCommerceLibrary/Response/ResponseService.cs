using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceLibrary.Response
{
    public class ResponsesService<T>
    {
        public bool Flag { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T? Data { get; set; }
        public ResponsesService() { }

        public ResponsesService(bool flag, string message, int statusCode, T? data)
        {
            Flag = flag;
            Message = message;
            StatusCode = statusCode;
            Data = data;
        }

        public static ResponsesService<T> Success(string message, int statusCode, T? data = default)
         => new() { Flag = true, Message = message, StatusCode = statusCode, Data = data };

        public static ResponsesService<T> Fail(string message, int statusCode, T? data = default)
            => new() { Flag = false, Message = message, StatusCode = statusCode, Data = data };
    }
}
