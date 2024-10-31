using System.ComponentModel.DataAnnotations;

namespace ProductProvider.Infrastructure.Data.Entities;

public class MaterialEntity
{
    [Key]
    public int MaterialID { get; set; }
    public string MaterialName { get; set; } = null!;

    public ICollection<ProductEntity> Products { get; set; } = [];
}
