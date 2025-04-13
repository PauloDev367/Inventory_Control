using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Requests;

public class RemoveSupplierToProductRequest
{
    [Required]
    public Guid ProductId { get; set; }
}