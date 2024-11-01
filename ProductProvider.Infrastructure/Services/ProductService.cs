using ProductProvider.Infrastructure.Data;
using ProductProvider.Infrastructure.Factories;
using ProductProvider.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

    public class ProductService(IDbContextFactory<DataContext> contextFactory, ILogger<ProductService> logger) : IProductService
    {
        private readonly IDbContextFactory<DataContext> _contextFactory = contextFactory;
        private readonly ILogger<ProductService> _logger = logger;

        public async Task<Product> CreateProductAsync(ProductCreateRequest request)
        {
            await using var context = _contextFactory.CreateDbContext();
            try
            {
                var productEntity = ProductFactory.Create(request);
                context.Products.Add(productEntity);
                await context.SaveChangesAsync();

                _logger.LogInformation($"Product created successfully with ID: {productEntity.Id}");
                return ProductFactory.Create(productEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating product: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            await using var context = _contextFactory.CreateDbContext();
            try
            {
                var productEntities = await context.Products.ToListAsync();
                _logger.LogInformation($"Retrieved {productEntities.Count} products.");
                return productEntities.Select(ProductFactory.Create);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving products: {ex.Message}");
                throw;
            }
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            await using var context = _contextFactory.CreateDbContext();
            try
            {
                var productEntity = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

                if (productEntity == null)
                {
                    _logger.LogWarning($"Product with Id {id} not found");
                    return null!;
                }

                _logger.LogInformation($"Retrieved Product: {productEntity.Id}, {productEntity.Title}");
                return ProductFactory.Create(productEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving product by ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<Product> UpdateProductAsync(ProductUpdateRequest request)
        {
            await using var context = _contextFactory.CreateDbContext();
            try
            {
                var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == request.Id);
                if (existingProduct == null)
                {
                    _logger.LogWarning($"Product with Id {request.Id} not found for update");
                    return null!;
                }

                var updateProductEntity = ProductFactory.Update(request);
                updateProductEntity.Id = existingProduct.Id;
                context.Entry(existingProduct).CurrentValues.SetValues(updateProductEntity);

                await context.SaveChangesAsync();
                _logger.LogInformation($"Product with ID {existingProduct.Id} updated successfully.");
                return ProductFactory.Create(existingProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product with ID {request.Id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            await using var context = _contextFactory.CreateDbContext();
            try
            {
                var product = await context.Products
                    .AsNoTracking()  // Avoid tracking for better compatibility with Cosmos DB
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    _logger.LogInformation($"Product with ID: {id} not found.");
                    return false;
                }

                // Clear owned entities to prevent tracking conflicts
                product.Categories = null;
                product.Materials = null;

                context.Products.Remove(product);
                await context.SaveChangesAsync();

                _logger.LogInformation($"Product deleted successfully with ID: {id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product with ID: {id} - {ex.Message}");
                throw;
            }
        }



    }
}
