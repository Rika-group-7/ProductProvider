namespace ProductProvider.Infrastructure.Models;

public class Product
{
    public string? ProductID { get; set; }
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

    public virtual List<Category>? Categories { get; set; }
    public virtual List<Material>? Materials { get; set; }
}

public class Category
{
    public int CategoryID { get; set; }
    public string? CategoryName { get; set; }
    public int? ParentCategoryID { get; set; }
}

public class Material
{
    public int MaterialID { get; set; }
    public string? MaterialName { get; set; }
}
