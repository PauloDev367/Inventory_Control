using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Requests;

public class UpdateUserRequest
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(50)]
    public string Password { get; set; }
}
