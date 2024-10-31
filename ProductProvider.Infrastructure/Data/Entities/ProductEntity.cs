using System.ComponentModel.DataAnnotations;

namespace ProductProvider.Infrastructure.Data.Entities;

public class ProductEntity
{
    [Key]
    public string ProductID { get; set; } = Guid.NewGuid().ToString();
    public string? Title { get; set; } 
    public string? Brand { get; set; }
    public string? Size { get; set; }
    public string? Color { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;
    public bool StockStatus { get; set; }
    public string SKU { get; set; } = null!;    
    public decimal Ratings { get; set; }
    public string? ProductImage { get; set; }

    public ICollection<CategoryEntity> Categories { get; set; } = [];
    public ICollection<MaterialEntity> Materials { get; set; } = [];
}
