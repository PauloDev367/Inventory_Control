using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace InventoryControl.Requests;

public class UpdateProductRequest
{
    [Required] 
    [MinLength(3)] 
    public String Name { get; set; }
    [Required] 
    [MinLength(10)] 
    public String Description { get; set; }
    [Required(AllowEmptyStrings = false)]
    public float Price { get; set; }
    [Required] 
    public int MinimumStock { get; set; }
}