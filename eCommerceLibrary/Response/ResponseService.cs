using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceLibrary.Response
{
    public class ResponsesService
    {
        public bool Flag { get; set; }
        public string Message { get; set; }

        public ResponsesService(bool flag, string message)
        {
            Flag = flag;
            Message = message;
        }
    }

}
