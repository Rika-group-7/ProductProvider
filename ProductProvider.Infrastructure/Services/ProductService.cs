using ProductProvider.Infrastructure.Data;
using ProductProvider.Infrastructure.Factories;
using ProductProvider.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductProvider.Infrastructure.Data.Context;
using ProductProvider.Infrastructure.Data.Entities;

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
                // Fetch the existing product without tracking
                var existingProduct = await context.Products
                    .AsNoTracking()  // Avoid tracking to prevent conflicts
                    .Include(p => p.Categories)
                    .Include(p => p.Materials)
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                if (existingProduct == null)
                {
                    _logger.LogWarning($"Product with Id {request.Id} not found for update");
                    return null!;
                }

                // Clear owned entities to prevent tracking conflicts
                existingProduct.Categories = null;
                existingProduct.Materials = null;

                // Update main product fields with values from the request or keep existing ones
                existingProduct.Title = request.Title ?? existingProduct.Title;
                existingProduct.Brand = request.Brand ?? existingProduct.Brand;
                existingProduct.Size = request.Size ?? existingProduct.Size;
                existingProduct.Color = request.Color ?? existingProduct.Color;
                existingProduct.Price = request.Price != 0 ? request.Price : existingProduct.Price;
                existingProduct.Description = request.Description ?? existingProduct.Description;
                existingProduct.StockStatus = request.StockStatus;
                existingProduct.SKU = request.SKU ?? existingProduct.SKU;
                existingProduct.Ratings = request.Ratings != 0 ? request.Ratings : existingProduct.Ratings;
                existingProduct.ProductImage = request.ProductImage ?? existingProduct.ProductImage;

                // Assign new categories from the request
                existingProduct.Categories = request.Categories!.Select(c => new CategoryEntity
                {
                    CategoryName = c.CategoryName,
                    SubCategories = c.SubCategories?.Select(sc => new CategoryEntity
                    {
                        CategoryName = sc.CategoryName
                    }).ToList() ?? new List<CategoryEntity>()
                }).ToList();

                // Assign new materials from the request
                existingProduct.Materials = request.Materials!.Select(m => new MaterialEntity
                {
                    MaterialName = m.MaterialName
                }).ToList();

                // Attach the modified product entity to the context
                context.Products.Update(existingProduct);

                // Save changes
                await context.SaveChangesAsync();

                _logger.LogInformation($"Product with ID {existingProduct.Id} updated successfully.");
                return ProductFactory.Create(existingProduct); // Convert to output model if needed
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
