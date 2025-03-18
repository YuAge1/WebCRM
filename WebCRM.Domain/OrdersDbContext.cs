using Microsoft.EntityFrameworkCore;
using WebCRM.Domain.Entities;

namespace WebCRM.Domain
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
        {
            if (Database.GetPendingMigrations().Any())
            {
                Database.Migrate();
            }
        }

        public DbSet<CustomerEntity> Customers { get; set; } = null!;
        public DbSet<CartEntity> Carts { get; set; } = null!;
        public DbSet<CartItemEntity> CartItems { get; set; } = null!;
        public DbSet<OrderEntity> Orders { get; set; } = null!;
    }
}
