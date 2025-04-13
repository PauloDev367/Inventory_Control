using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Requests;

public class CreateSupplierRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public string Name { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    [Phone]
    public string? PhoneNumber { get; set; }
}