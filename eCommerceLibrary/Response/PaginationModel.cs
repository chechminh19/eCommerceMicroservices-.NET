using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceLibrary.Response
{
    public class PaginationModel<T>
    {
        public int Page { get; set; }
        public int TotalPage { get; set; }
        public int TotalRecords { get; set; }
        public T ListData { get; set; }
    }
}
