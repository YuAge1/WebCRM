using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCRM.Application.Models.Orders;

namespace WebCRM.Application.Abstractions
{
    public interface IOrderService
    {
        Task<OrderDto> Create(CreateOrderDto order);
        Task<OrderDto> GetById(long orderId);
        Task<List<OrderDto>> GetByUser(long customerId);
        Task<List<OrderDto>> GetAll();
        Task Reject(long orderId);
    }
}
