using Microsoft.EntityFrameworkCore;

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
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>().ToContainer("Products");
        modelBuilder.Entity<ProductEntity>().HasKey(p => p.Id);

        modelBuilder.Entity<ProductEntity>()
            .OwnsMany(p => p.Categories, cb =>
            {
                cb.HasKey(c => c.Id);  
                cb.OwnsMany(c => c.SubCategories, scb =>
                {
                    scb.HasKey(sc => sc.Id); 
                });
            });

        modelBuilder.Entity<ProductEntity>().OwnsMany(p => p.Materials, mb =>
        {
            mb.HasKey(m => m.Id);  
        });
    }



}
