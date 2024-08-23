using Microsoft.EntityFrameworkCore;
using orderit_api.Models;

namespace orderit_api.Data
{
    public class BusinessContext : DbContext
    {
        public BusinessContext(DbContextOptions<BusinessContext> options) : base(options)
        {

        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Salesperson> Salespersons { get; set;}
        public DbSet<Store> Stores { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                // ProductId como clave primaria
                entity.HasKey(p => p.ProductId);

                // Incremento automático del id
                entity.Property(p => p.ProductId)
                    .ValueGeneratedOnAdd();

            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.OrderId);
                entity.Property(o => o.OrderId)
                    .ValueGeneratedOnAdd();
            
            });
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(od => new { od.OrderId, od.OrderDetailId });
                entity.Property(o => o.OrderDetailId)
                    .ValueGeneratedOnAdd();
                
                entity.HasOne(od => od.Order)
                    .WithMany(o => o.Details)
                    .HasForeignKey(od => od.OrderId);


            });
            modelBuilder.Entity<Store>(entity =>    
            {
                entity.HasKey(o => o.StoreId);
                entity.Property(o => o.StoreId)
                    .ValueGeneratedOnAdd();

            });
            modelBuilder.Entity<Salesperson>(entity =>
            {
                entity.HasKey(o => o.SalespersonId);
                entity.Property(o => o.SalespersonId)
                    .ValueGeneratedOnAdd();

            });

        }

    }


}
