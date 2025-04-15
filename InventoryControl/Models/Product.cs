using System.Text.Json.Serialization;

namespace InventoryControl.Models;

public class Product
{
    public Guid Id { get; set; }
    public String Name { get; set; }
    public String Description { get; set; }
    public int Quantity { get; set; }
    public float Price { get; set; }
    public int MinimumStock { get; set; }
    [JsonIgnore]
    public List<Category> Categories { get; set; }
    [JsonIgnore] 
    public Supplier? Supplier { get; set; }
    [JsonIgnore] public List<ProductPrice> ProductPrices { get; set; }
    public Guid? SupplierId { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DeletedAt { get; set; }
}