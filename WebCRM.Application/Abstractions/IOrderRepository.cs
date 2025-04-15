using WebCRM.Domain.Entities;

namespace WebCRM.Application.Abstractions;

public interface IOrderRepository
{
    Task<OrderEntity?> GetById(long id);
    Task<IEnumerable<OrderEntity>> GetCustomerOrders(long customerId);
    Task<CustomerEntity?> GetCustomerById(long customerId);
    Task Update(OrderEntity order);
} 