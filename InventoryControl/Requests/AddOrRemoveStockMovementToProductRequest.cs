using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Requests;

public class AddOrRemoveStockMovementToProductRequest
{  
    [Required]
    public int Quantity { get; set; }
}