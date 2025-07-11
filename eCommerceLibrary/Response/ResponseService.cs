using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceLibrary.Response
{
    public class ResponsesServiceDTO<T>
    {
        public bool Flag { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ResponsesServiceDTO(bool flag, string message, T data)
        {
            Flag = flag;
            Message = message;
            Data = data;
        }
    }

    public class ResponsesService
    {
        public bool Flag { get; set; }
        public string Message { get; set; }

        public ResponsesService(bool flag, string message)
        {
            Flag = flag;
            Message = message;
        }

        //public static ResponsesService Success(string message = "Success")
        //    => new ResponsesService(true, message);

        //public static ResponsesService Fail(string message = "Failed")
        //    => new ResponsesService(false, message);

        //public static ResponsesServiceDTO<T> Success<T>(T data, string message = "Success")
        //    => new ResponsesServiceDTO<T>(true, message, data);

        //public static ResponsesServiceDTO<T> Fail<T>(string message = "Failed")
        //    => new ResponsesServiceDTO<T>(false, message, default);
    }
}
