namespace InventoryControl.Models;

public class Product
{
    public Guid Id { get; set; }
    public String Name { get; set; }
    public String Description { get; set; }
    public int Quantity { get; set; }
    public float Price { get; set; }
    public DateTime CreatedAt { get; private set; }
}