using Microsoft.EntityFrameworkCore;
using ProductProvider.Infrastructure.Data.Entities;

namespace ProductProvider.Infrastructure.Data.Context;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<MaterialEntity> Materials { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.EnableSensitiveDataLogging();

        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>().ToContainer("Products");
        modelBuilder.Entity<ProductEntity>().HasKey(p => p.Id);

        modelBuilder.Entity<ProductEntity>()
            .OwnsMany(p => p.Categories, cb =>
            {
                cb.OwnsMany(c => c.SubCategories);
            });

        modelBuilder.Entity<ProductEntity>().OwnsMany(p => p.Materials);
    }




}
