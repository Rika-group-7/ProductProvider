using Microsoft.EntityFrameworkCore;
using ProductProvider.Infrastructure.Data.Entities;

namespace ProductProvider.Test.Data;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
    
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<MaterialEntity> Materials { get; set; }

}
