using System.ComponentModel.DataAnnotations;

namespace CompanyInventory.DTOs;

public class ProductDto
{

    public int ProductId { get; set; }

    [Required]
    [Display(Name = "Product Name")]
    public string ProductName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Required]
    [Display(Name = "Material Cost")]
    [Range(0, double.MaxValue)]
    public decimal MaterialCost { get; set; }

    [Required]
    [Display(Name = "Service Cost")]
    [Range(0, double.MaxValue)]
    public decimal ServiceCost { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    public string? Description { get; set; }
}