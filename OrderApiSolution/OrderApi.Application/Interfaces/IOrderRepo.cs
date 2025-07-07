using eCommerceLibrary.Generic;
using OrderApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Interfaces
{
    public interface IOrderRepo : IGenericRepo<Order>
    {
        //add more method
    }
}
