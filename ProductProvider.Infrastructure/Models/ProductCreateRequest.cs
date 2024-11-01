namespace ProductProvider.Infrastructure.Models;

public class ProductCreateRequest
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

    public List<CategoryCreateRequest>? Categories { get; set; } = new(); 
    public List<MaterialCreateRequest>? Materials { get; set; } = new();  

public class CategoryCreateRequest
{
    public string? Id { get; set; }
    public string? CategoryName { get; set; }
    public List<CategoryCreateRequest>? SubCategories { get; set; } = new();
}

public class MaterialCreateRequest
{
    public string? Id { get; set; }
    public string? MaterialName { get; set; }
}
