namespace ProductProvider.Infrastructure.Models;

public class Product
{
    public string? Id { get; set; } 
    public string? Title { get; set; }
    public string? Brand { get; set; }
    public string? Size { get; set; }
    public string? Color { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public bool StockStatus { get; set; }
    public string? SKU { get; set; }
    public decimal Ratings { get; set; }
    public string? ProductImage { get; set; }

    public List<Category>? Categories { get; set; } = [];
    public List<Material>? Materials { get; set; } = [];
}

public class Category
{
    public string? CategoryName { get; set; }

    public List<Category>? SubCategories { get; set; } = [];
}

public class Material
{
    public string? MaterialName { get; set; }
}
