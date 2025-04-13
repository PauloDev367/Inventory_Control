using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Requests;

public class AddCategoryToProductRequest
{
    [Required] 
    public List<Guid> CategoriesId { get; set; }
}