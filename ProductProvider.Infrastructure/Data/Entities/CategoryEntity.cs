using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductProvider.Infrastructure.Data.Entities;

public class CategoryEntity
{
    [Key]
    public int CategoryID { get; set; }

    public string CategoryName { get; set; } = null!;

    public int? ParentCategoryID { get; set; }

    [ForeignKey("ParentCategoryID")]
    public CategoryEntity? ParentCategory { get; set; }

    public ICollection<CategoryEntity> SubCategories { get; set; } = [];
    public ICollection<ProductEntity> Products { get; set; } = [];
}
