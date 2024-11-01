using System.ComponentModel.DataAnnotations;

public class MaterialEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();  // Unique ID for each material
    public string? MaterialName { get; set; }
}
