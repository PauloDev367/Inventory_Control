using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Requests;

public class RemoveCategoryToProductRequest
{
    [Required] 
    public List<Guid> CategoriesId { get; set; }
}