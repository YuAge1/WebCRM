using Microsoft.EntityFrameworkCore;
using WebCRM.Application.Abstractions;
using WebCRM.Application.Mappers;
using WebCRM.Application.Models.Orders;
using WebCRM.Domain;
using WebCRM.Domain.Entities;
using WebCRM.Domain.Exceptions;

namespace WebCRM.Application.Services
{
    public class OrdersService(OrdersDbContext context, ICartsService cartsService) : IOrderService
    {
        public async Task<OrderDto> Create(CreateOrderDto order)
        {
            var orderByOrderNumber = await context.Orders.FirstOrDefaultAsync(x => 
                x.OrderNumber == order.OrderNumber && x.MerchantId == order.MerchantId);

            if (orderByOrderNumber != null)
            {
                throw new DuplicateEntityException($"Order with orderNumber {order.OrderNumber} is already exists for merchant " +
                                                   $"{order.MerchantId}");
            }
            
            if (order.Cart == null)
            {
                throw new ArgumentNullException();
            }
            
            var cart = await cartsService.Create(order.Cart);
            var entity = new OrderEntity
            {
                OrderNumber = order.OrderNumber,
                Name = order.Name,
                CustomerId = order.CustomerId,
                CartId = cart.Id
            };

            var orderSaveChanges = await context.Orders.AddAsync(entity);
            await context.SaveChangesAsync();

            var orderEntityResult = orderSaveChanges.Entity;

            return orderEntityResult.ToDto();
        }

        public async Task<List<OrderDto>> GetAll()
        {
            var entity = await context.Orders
                .Include(x => x.Cart)
                .ThenInclude(c => c.CartItems)
                .ToListAsync();
            
            return entity.Select(x => x.ToDto()).ToList();
        }

        public async Task<OrderDto> GetById(long orderId)
        {
            var entity = await context.Orders
                .Include(x => x.Cart)
                .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync(x => x.Id == orderId);

            if (entity == null)
            {
                throw new EntityNotFoundException($"Order entity with id {orderId} not found");
            }
            
            return entity.ToDto();
        }

        public async Task<List<OrderDto>> GetByUser(long customerId)
        {
            var entity = await context.Orders
                .Include(o => o.Cart)
                .ThenInclude(c => c.CartItems)
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();
            
            return entity.Select(x => x.ToDto()).ToList();
        }

        //todo: прикрутить статусную модель для отмены заказа
        public Task Reject(long orderId)
        {
            throw new NotImplementedException();
        }
    }
}
