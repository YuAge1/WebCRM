using Microsoft.EntityFrameworkCore;
using WebCRM.Application.Abstractions;
using WebCRM.Application.Mappers;
using WebCRM.Application.Models.Carts;
using WebCRM.Domain;
using WebCRM.Domain.Entities;

namespace WebCRM.Application.Services
{
    public class CartsService(OrdersDbContext context) : ICartsService
    {
        public async Task<CartDto> Create(CartDto cart)
        {
            var cartEntity = new CartEntity();
            var cartSaveResult = await context.Carts.AddAsync(cartEntity);
            await context.SaveChangesAsync();

            var cartItems = cart.CartItems
                .Select(item => new CartItemEntity
                {
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    CartId = cartSaveResult.Entity.Id
                });
            await context.CartItems.AddRangeAsync(cartItems);
            await context.SaveChangesAsync();

            var result = await context.Carts
                .Include(x => x.CartItems)
                .FirstAsync(x => x.Id == cartSaveResult.Entity.Id);

            return result.ToDto();
        }
    }
}
