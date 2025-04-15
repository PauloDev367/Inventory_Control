using System.Text.Json.Serialization;

namespace InventoryControl.Models;

public class ProductPrice
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    [JsonIgnore]
    public Product Product { get; set; }
    public float Price { get; set; }
    public DateTime CreatedAt { get; private set; }
}