using InventoryControl.Enums;

namespace InventoryControl.Requests;

public class CreateStockMovementRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public MovementType MovementType { get; set; }
}