using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebCRM.Domain.Entities;

namespace WebCRM.Domain
{
    public sealed class OrdersDbContext : IdentityDbContext<UserEntity, IdentityRoleEntity, long>
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
        public DbSet<MerchantEntity> Merchants { get; set; } = null!;
        public DbSet<LeadEntity> Leads { get; set; } = null!;
        public DbSet<DealEntity> Deals { get; set; } = null!;
        public DbSet<TaskEntity> Tasks { get; set; } = null!;
        public DbSet<ActivityEntity> Activities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация связей и индексов
            modelBuilder.Entity<LeadEntity>()
                .HasOne(l => l.AssignedToUser)
                .WithMany()
                .HasForeignKey(l => l.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DealEntity>()
                .HasOne(d => d.Customer)
                .WithMany()
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DealEntity>()
                .HasOne(d => d.AssignedToUser)
                .WithMany()
                .HasForeignKey(d => d.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DealEntity>()
                .HasOne(d => d.Lead)
                .WithMany()
                .HasForeignKey(d => d.LeadId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskEntity>()
                .HasOne(t => t.AssignedToUser)
                .WithMany()
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActivityEntity>()
                .HasOne(a => a.CreatedByUser)
                .WithMany()
                .HasForeignKey(a => a.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка первичных ключей для всех сущностей
            modelBuilder.Entity<CustomerEntity>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<MerchantEntity>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<OrderEntity>()
                .HasKey(o => o.Id);

            modelBuilder.Entity<CartEntity>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<CartItemEntity>()
                .HasKey(ci => ci.Id);

            modelBuilder.Entity<LeadEntity>()
                .HasKey(l => l.Id);

            modelBuilder.Entity<DealEntity>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<TaskEntity>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<ActivityEntity>()
                .HasKey(a => a.Id);
        }
    }
}
