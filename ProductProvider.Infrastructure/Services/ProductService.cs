using ProductProvider.Infrastructure.Data;
using ProductProvider.Infrastructure.Factories;
using ProductProvider.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using ProductProvider.Infrastructure.Data.Context;

namespace ProductProvider.Infrastructure.Services
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(ProductCreateRequest request);
        Task<Product> GetProductByIdAsync(string id);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> UpdateProductAsync(ProductUpdateRequest request);
        Task<bool> DeleteProductAsync(string id);
    }

    public class ProductService : IProductService
    {
        private readonly IDbContextFactory<DataContext> _contextFactory;

        public ProductService(IDbContextFactory<DataContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Product> CreateProductAsync(ProductCreateRequest request)
        {
            await using var context = _contextFactory.CreateDbContext();

            var productEntity = ProductFactory.Create(request);
            context.Products.Add(productEntity);
            await context.SaveChangesAsync();

            return ProductFactory.Create(productEntity);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            await using var context = _contextFactory.CreateDbContext();
            var productEntities = await context.Products.ToListAsync();
            return productEntities.Select(ProductFactory.Create);
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            await using var context = _contextFactory.CreateDbContext();
            var productEntity = await context.Products.FirstOrDefaultAsync(p => p.ProductID == id);

            if (productEntity == null)
            {
                Console.WriteLine($"Product with Id {id} not found");
                return null!;
            }

            Console.WriteLine($"Retrieved Product: {productEntity.ProductID}, {productEntity.Title}");
            return ProductFactory.Create(productEntity);
        }

        public async Task<Product> UpdateProductAsync(ProductUpdateRequest request)
        {
            await using var context = _contextFactory.CreateDbContext();
            var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.ProductID == request.ProductID);
            if (existingProduct == null) return null!;

            var updateProductEntity = ProductFactory.Update(request);
            updateProductEntity.ProductID = existingProduct.ProductID;
            context.Entry(existingProduct).CurrentValues.SetValues(updateProductEntity);

            await context.SaveChangesAsync();
            return ProductFactory.Create(existingProduct);
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            await using var context = _contextFactory.CreateDbContext();
            var productEntity = await context.Products.FirstOrDefaultAsync(p => p.ProductID == id);
            if (productEntity == null) return false;

            context.Products.Remove(productEntity);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
