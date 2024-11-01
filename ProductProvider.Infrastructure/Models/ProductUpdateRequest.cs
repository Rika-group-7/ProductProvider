namespace ProductProvider.Infrastructure.Models;
public class ProductUpdateRequest
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

    public List<CategoryUpdateRequest>? Categories { get; set; } = []; 
    public List<MaterialUpdateRequest>? Materials { get; set; } = [];  
}

public class CategoryUpdateRequest
{
    public string? Id { get; set; }
    public string? CategoryName { get; set; }
    public List<CategoryUpdateRequest>? SubCategories { get; set; } = [];
}
public class MaterialUpdateRequest
{
    public string? Id { get; set; }
    public string? MaterialName { get; set; }
}