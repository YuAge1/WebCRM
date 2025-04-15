using Microsoft.EntityFrameworkCore;
using WebCRM.Application.Abstractions;
using WebCRM.Domain;
using WebCRM.Domain.Entities;

namespace WebCRM.Application.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrdersDbContext _context;

    public OrderRepository(OrdersDbContext context)
    {
        _context = context;
    }

    public async Task<OrderEntity?> GetById(long id)
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<OrderEntity>> GetCustomerOrders(long customerId)
    {
        return await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<CustomerEntity?> GetCustomerById(long customerId)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == customerId);
    }

    public async Task Update(OrderEntity order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }
} 