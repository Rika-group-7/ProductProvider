using ProductProvider.Infrastructure.Data.Entities;
using ProductProvider.Infrastructure.Models;

namespace ProductProvider.Infrastructure.Factories;

public static class ProductFactory
{
    public static ProductEntity Create(ProductCreateRequest request)
    {
        return new ProductEntity
        {
            Id = Guid.NewGuid().ToString(),
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
                CategoryName = c.CategoryName,
                SubCategories = c.SubCategories?.Select(sc => new CategoryEntity
                {
                    CategoryName = sc.CategoryName,
                    SubCategories = new List<CategoryEntity>()
                }).ToList() ?? new List<CategoryEntity>()
            }).ToList() ?? new List<CategoryEntity>(),
            Materials = request.Materials?.Select(m => new MaterialEntity
            {
                MaterialName = m.MaterialName
            }).ToList() ?? new List<MaterialEntity>()
        };
    }

    public static Product Create(ProductEntity entity)
    {
        return new Product
        {
            Id = entity.Id,
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
                CategoryName = c.CategoryName,
                SubCategories = c.SubCategories?.Select(sc => new Category
                {
                    CategoryName = sc.CategoryName
                }).ToList()
            }).ToList(),
            Materials = entity.Materials?.Select(m => new Material
            {
                MaterialName = m.MaterialName
            }).ToList()
        };
    }
}

