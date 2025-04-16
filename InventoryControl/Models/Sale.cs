using System.Text.Json.Serialization;

namespace InventoryControl.Models;

public class Sale
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    [JsonIgnore]
    public Product Product { get; set; }
    public float TotalPrice { get; set; }
    public int Quantity { get; set; }
    public float UnityPrice { get; set; }
    public DateTime CreatedAt { get; private set; }
}