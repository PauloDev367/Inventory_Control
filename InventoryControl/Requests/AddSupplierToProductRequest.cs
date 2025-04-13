using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Requests;

public class AddSupplierToProductRequest
{
    [Required]
    public Guid ProductId { get; set; }
}