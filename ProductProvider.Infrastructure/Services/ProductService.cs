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
                var existingProduct = await context.Products
                    .Include(p => p.Categories)
                    .Include(p => p.Materials)
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                if (existingProduct == null)
                {
                    _logger.LogWarning($"Product with Id {request.Id} not found for update");
                    return null!;
                }

                // Update main product fields only if they are non-null in the request
                if (request.Title != null) existingProduct.Title = request.Title;
                if (request.Brand != null) existingProduct.Brand = request.Brand;
                if (request.Size != null) existingProduct.Size = request.Size;
                if (request.Color != null) existingProduct.Color = request.Color;
                if (request.Price != 0) existingProduct.Price = request.Price;
                if (request.Description != null) existingProduct.Description = request.Description;
                existingProduct.StockStatus = request.StockStatus;
                if (request.SKU != null) existingProduct.SKU = request.SKU;
                if (request.Ratings != 0) existingProduct.Ratings = request.Ratings;
                if (request.ProductImage != null) existingProduct.ProductImage = request.ProductImage;

                // Update Categories: Remove old ones not in the request, update existing, and add new ones
                if (request.Categories != null)
                {
                    // Remove categories not present in the request
                    existingProduct.Categories!.RemoveAll(c =>
                        request.Categories.All(rc => rc.CategoryName != c.CategoryName && rc.Id != c.Id));

                    foreach (var categoryRequest in request.Categories)
                    {
                        var existingCategory = existingProduct.Categories
                            .FirstOrDefault(c => (c.Id == categoryRequest.Id && categoryRequest.Id != null)
                                                 || c.CategoryName == categoryRequest.CategoryName);

                        if (existingCategory != null)
                        {
                            // Update existing category name if provided
                            if (categoryRequest.CategoryName != null)
                                existingCategory.CategoryName = categoryRequest.CategoryName;

                            // Update SubCategories, avoiding duplicates by name and removing missing ones
                            existingCategory.SubCategories!.RemoveAll(sc =>
                                categoryRequest.SubCategories!.All(rsc => rsc.CategoryName != sc.CategoryName && rsc.Id != sc.Id));

                            foreach (var subCategoryRequest in categoryRequest.SubCategories ?? new List<ProductUpdateRequest.CategoryUpdateRequest>())
                            {
                                var existingSubCategory = existingCategory.SubCategories
                                    .FirstOrDefault(sc => (sc.Id == subCategoryRequest.Id && subCategoryRequest.Id != null)
                                                          || sc.CategoryName == subCategoryRequest.CategoryName);

                                if (existingSubCategory != null)
                                {
                                    if (subCategoryRequest.CategoryName != null)
                                        existingSubCategory.CategoryName = subCategoryRequest.CategoryName;
                                }
                                else
                                {
                                    existingCategory.SubCategories.Add(new CategoryEntity
                                    {
                                        Id = subCategoryRequest.Id ?? Guid.NewGuid().ToString(),
                                        CategoryName = subCategoryRequest.CategoryName
                                    });
                                }
                            }
                        }
                        else
                        {
                            // Add new category if it doesn't exist by name
                            existingProduct.Categories.Add(new CategoryEntity
                            {
                                Id = categoryRequest.Id ?? Guid.NewGuid().ToString(),
                                CategoryName = categoryRequest.CategoryName,
                                SubCategories = categoryRequest.SubCategories?.Select(sc => new CategoryEntity
                                {
                                    Id = sc.Id ?? Guid.NewGuid().ToString(),
                                    CategoryName = sc.CategoryName
                                }).ToList() ?? new List<CategoryEntity>()
                            });
                        }
                    }
                }

                // Update Materials: Remove old ones not in the request, update existing, and add new ones
                if (request.Materials != null)
                {
                    // Remove materials not present in the request
                    existingProduct.Materials!.RemoveAll(m =>
                        request.Materials.All(rm => rm.MaterialName != m.MaterialName && rm.Id != m.Id));

                    // Update or add materials
                    foreach (var materialRequest in request.Materials)
                    {
                        var existingMaterial = existingProduct.Materials
                            .FirstOrDefault(m => (m.Id == materialRequest.Id && materialRequest.Id != null)
                                                 || m.MaterialName == materialRequest.MaterialName);

                        if (existingMaterial != null)
                        {
                            if (materialRequest.MaterialName != null)
                                existingMaterial.MaterialName = materialRequest.MaterialName;
                        }
                        else
                        {
                            existingProduct.Materials.Add(new MaterialEntity
                            {
                                Id = materialRequest.Id ?? Guid.NewGuid().ToString(),
                                MaterialName = materialRequest.MaterialName
                            });
                        }
                    }
                }

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
