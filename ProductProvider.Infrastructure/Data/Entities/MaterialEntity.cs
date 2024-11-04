using System.ComponentModel.DataAnnotations;
namespace ProductProvider.Infrastructure.Data.Entities;
public class MaterialEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString(); 
    public string? MaterialName { get; set; }
}
