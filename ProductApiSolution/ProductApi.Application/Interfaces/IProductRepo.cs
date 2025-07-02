using eCommerceLibrary.Generic;
using eCommerceLibrary.Response;
using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.Interfaces
{
    public interface IProductRepo : IGenericRepo<Product>
    {
        //Add more method if need
    }
}
        