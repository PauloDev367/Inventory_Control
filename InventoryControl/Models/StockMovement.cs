using InventoryControl.Enums;

namespace InventoryControl.Models;

public class StockMovement
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public MovementType MovementType { get; set; }
    public DateTime CreatedAt { get; private set; }
}