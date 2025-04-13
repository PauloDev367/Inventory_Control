using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Requests;

public class CreateCategoryRequest
{
    [Required] 
    [MinLength(3)] 
    public String Name { get; set; }
}