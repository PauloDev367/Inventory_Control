using System.Text.Json.Serialization;

namespace InventoryControl.Models;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public List<Product> Products { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DeletedAt { get; set; }
}