using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Requests;

public class UpdateCategoryRequest
{
    [Required] 
    [MinLength(3)] 
    public String Name { get; set; }
}