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

    public class ProductService : IProductService
    {
        private readonly IDbContextFactory<DataContext> _contextFactory;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IDbContextFactory<DataContext> contextFactory, ILogger<ProductService> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

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
                var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductID);
                if (existingProduct == null)
                {
                    _logger.LogWarning($"Product with Id {request.ProductID} not found for update");
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
                _logger.LogError($"Error updating product with ID {request.ProductID}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            await using var context = _contextFactory.CreateDbContext();
            try
            {
                var productEntity = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (productEntity == null)
                {
                    _logger.LogWarning($"Product with Id {id} not found for deletion");
                    return false;
                }

                context.Products.Remove(productEntity);
                await context.SaveChangesAsync();
                _logger.LogInformation($"Product with ID {id} deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product with ID {id}: {ex.Message}");
                throw;
            }
        }
    }
}
