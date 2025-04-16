using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Requests;

public class CreateSaleRequest
{
    [Required] public Guid ProductId { get; set; }
    [Required] public int Quantity { get; set; }
    [Required] public float UnityPrice { get; set; }
}