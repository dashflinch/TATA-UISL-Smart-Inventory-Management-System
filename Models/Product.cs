using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyInventory.Models;

public class Product : BaseEntity
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [StringLength(150)]
    public string ProductName { get; set; } = string.Empty;

    [Required]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal MaterialCost { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ServiceCost { get; set; }

    [NotMapped]
    public decimal TotalCost => MaterialCost + ServiceCost;

    [Required]
    public int Quantity { get; set; }

    [StringLength(250)]
    public string? Description { get; set; }
}