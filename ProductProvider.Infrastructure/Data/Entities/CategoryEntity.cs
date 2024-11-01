using System.ComponentModel.DataAnnotations;
namespace ProductProvider.Infrastructure.Data.Entities;
public class CategoryEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();  // Unique ID for each category
    public string? CategoryName { get; set; }
    public virtual List<CategoryEntity>? SubCategories { get; set; } = new(); // Embed subcategories as a nested list with unique IDs
}