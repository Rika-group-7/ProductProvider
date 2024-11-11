using System.ComponentModel.DataAnnotations;
namespace ProductProvider.Infrastructure.Data.Entities;
public class MaterialEntity
{
    [Key]
    public string? MaterialName { get; set; }
}
