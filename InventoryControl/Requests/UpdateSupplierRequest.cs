using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Requests;

public class UpdateSupplierRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    [Phone]
    public string? PhoneNumber { get; set; }
}