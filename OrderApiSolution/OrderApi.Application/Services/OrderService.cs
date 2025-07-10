using eCommerceLibrary.Response;
using OrderApi.Application.DTOs;
using OrderApi.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        public OrderService(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<ResponsesService> CreateAsync(OrderCreateDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponsesService> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponsesService> UpdateAsync(OrderUpdateDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
