using System.Linq;
using ProductProvider.Infrastructure.Data.Entities;
using ProductProvider.Infrastructure.Models;

namespace ProductProvider.Infrastructure.Factories;

public static class ProductFactory
{
    public static ProductEntity Create(ProductCreateRequest request)
    {
        return new ProductEntity
        {
            ProductID = request.ProductID ?? Guid.NewGuid().ToString(),
            Title = request.Title,
            Brand = request.Brand,
            Size = request.Size,
            Color = request.Color,
            Price = request.Price,
            Description = request.Description,
            StockStatus = request.StockStatus,
            SKU = request.SKU,
            Ratings = request.Ratings,
            ProductImage = request.ProductImage,
            Categories = request.Categories?.Select(c => new CategoryEntity
            {
                CategoryName = c.CategoryName
            }).ToList() ?? new List<CategoryEntity>(),
            Materials = request.Materials?.Select(m => new MaterialEntity
            {
                MaterialName = m.MaterialName
            }).ToList() ?? new List<MaterialEntity>()
        };
    }

    public static ProductEntity Update(ProductUpdateRequest request)
    {
        return new ProductEntity
        {
            ProductID = request.ProductID,
            Title = request.Title,
            Brand = request.Brand,
            Size = request.Size,
            Color = request.Color,
            Price = request.Price,
            Description = request.Description,
            StockStatus = request.StockStatus,
            SKU = request.SKU,
            Ratings = request.Ratings,
            ProductImage = request.ProductImage,
            Categories = request.Categories?.Select(c => new CategoryEntity
            {
                CategoryID = c.CategoryID != null ? int.Parse(c.CategoryID) : 0,
                CategoryName = c.CategoryName
            }).ToList() ?? new List<CategoryEntity>(),
            Materials = request.Materials?.Select(m => new MaterialEntity
            {
                MaterialID = m.MaterialID != null ? int.Parse(m.MaterialID) : 0,
                MaterialName = m.MaterialName
            }).ToList() ?? new List<MaterialEntity>()
        };
    }

    public static Product Create(ProductEntity entity)
    {
        return new Product
        {
            ProductID = entity.ProductID,
            Title = entity.Title,
            Brand = entity.Brand,
            Size = entity.Size,
            Color = entity.Color,
            Price = entity.Price,
            Description = entity.Description,
            StockStatus = entity.StockStatus,
            SKU = entity.SKU,
            Ratings = entity.Ratings,
            ProductImage = entity.ProductImage,
            Categories = entity.Categories?.Select(c => new Category
            {
                CategoryID = c.CategoryID,
                CategoryName = c.CategoryName
            }).ToList(),
            Materials = entity.Materials?.Select(m => new Material
            {
                MaterialID = m.MaterialID,
                MaterialName = m.MaterialName
            }).ToList()
        };
    }
}
