using System.ComponentModel.DataAnnotations;

namespace CompanyInventory.Models;

public class Category : BaseEntity
{
    [Key]
    public int CategoryId { get; set; }

    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Description { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}