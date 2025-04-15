using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Requests;

public class CreateProductPriceRequest
{
    [Required]
    public Guid ProductId { get; set; }
    [Required]  
    public float Price { get; set; }
}