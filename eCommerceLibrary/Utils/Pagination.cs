using eCommerceLibrary.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceLibrary.Utils
{
    public class Pagination
    {
        public static async Task<PaginationModel<IEnumerable<T>>> GetPagination<T>(IEnumerable<T> list, int page, int pageSize)
        {
            var startIndex = (page - 1) * pageSize;
            var currentPageData = list.Skip(startIndex).Take(pageSize).ToList();
            await Task.Delay(1);
            var paginationModel = new PaginationModel<IEnumerable<T>>
            {
                Page = page,
                TotalRecords = list.Count(),
                TotalPage = (int)Math.Ceiling(list.Count() / (double)pageSize),
                ListData = currentPageData
            };
            return paginationModel;
        }
    }
}
