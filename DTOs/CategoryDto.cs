using System.ComponentModel.DataAnnotations;

namespace CompanyInventory.DTOs;

public class CategoryDto
{

    public int CategoryId { get; set; }

    [Required]
    [Display(Name = "Category Name")]
    public string CategoryName { get; set; } = string.Empty;

    public string? Description { get; set; }
}