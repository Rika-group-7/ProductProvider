using Microsoft.EntityFrameworkCore;
using ProductProvider.Infrastructure.Data.Entities;

namespace ProductProvider.Infrastructure.Data.Context;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<ProductEntity> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>().ToContainer("Products");
        modelBuilder.Entity<ProductEntity>().HasPartitionKey(c => c.ProductID);
        modelBuilder.Entity<ProductEntity>().OwnsMany(c => c.Categories);
        modelBuilder.Entity<ProductEntity>().OwnsMany(c => c.Materials);
    }
}
