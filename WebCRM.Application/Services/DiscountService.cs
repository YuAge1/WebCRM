using WebCRM.Application.Abstractions;
using WebCRM.Domain.Models;
using WebCRM.Domain.Exceptions;

namespace WebCRM.Application.Services;

public interface IDiscountService
{
    Task<Discount> CreateDiscount(Discount discount);
    Task<Discount> GetDiscount(long id);
    Task<IEnumerable<Discount>> GetActiveDiscounts();
    Task<decimal> CalculateCustomerDiscount(long customerId);
    Task ApplyDiscountToOrder(long orderId, long customerId);
}

public class DiscountService : IDiscountService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEmailService _emailService;
    private readonly List<Discount> _discounts = new();

    public DiscountService(IOrderRepository orderRepository, IEmailService emailService)
    {
        _orderRepository = orderRepository;
        _emailService = emailService;
    }

    public async Task<Discount> CreateDiscount(Discount discount)
    {
        discount.Id = _discounts.Count + 1;
        _discounts.Add(discount);
        return discount;
    }

    public async Task<Discount> GetDiscount(long id)
    {
        var discount = _discounts.FirstOrDefault(d => d.Id == id);
        if (discount == null)
            throw new EntityNotFoundException($"Discount with id {id} not found");
        return discount;
    }

    public async Task<IEnumerable<Discount>> GetActiveDiscounts()
    {
        return _discounts.Where(d => d.IsActive && d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now);
    }

    public async Task<decimal> CalculateCustomerDiscount(long customerId)
    {
        var orders = await _orderRepository.GetCustomerOrders(customerId);
        var orderCount = orders.Count();

        var applicableDiscount = _discounts
            .Where(d => d.IsActive && d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now)
            .OrderByDescending(d => d.Percentage)
            .FirstOrDefault(d => orderCount >= d.MinimumOrders);

        return applicableDiscount?.Percentage ?? 0;
    }

    public async Task ApplyDiscountToOrder(long orderId, long customerId)
    {
        var discount = await CalculateCustomerDiscount(customerId);
        if (discount > 0)
        {
            var order = await _orderRepository.GetById(orderId);
            if (order != null)
            {
                order.Discount = discount;
                await _orderRepository.Update(order);

                // Отправляем уведомление о скидке
                var customer = await _orderRepository.GetCustomerById(customerId);
                if (customer != null && !string.IsNullOrEmpty(customer.Email))
                {
                    await _emailService.SendDiscountNotification(customer.Email, discount);
                }
            }
        }
    }
} 