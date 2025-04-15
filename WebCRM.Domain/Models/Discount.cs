namespace WebCRM.Domain.Models;

public class Discount
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Percentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public int MinimumOrders { get; set; } // Минимальное количество заказов для получения скидки
} 