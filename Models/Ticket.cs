using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyInventory.Models;

public class Ticket : BaseEntity
{
    [Key]
    public int TicketId { get; set; }

    [Required]
    [StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    // Application / Network / Hardware
    [Required]
    [StringLength(50)]
    public string IssueCategory { get; set; } = string.Empty;

    // Login Issue, VPN, Printer, etc.
    [Required]
    [StringLength(100)]
    public string IssueType { get; set; } = string.Empty;

    // SAP Portal, HRMS, Printer-01, 10.10.1.25
    [Required]
    [StringLength(200)]
    public string AffectedSystem { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Impact { get; set; } = string.Empty;

    // Optional link to inventory
    public int? ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    [Required]
    public string Priority { get; set; } = "Medium";

    [Required]
    public string Status { get; set; } = "Open";

    public string? AssignedTo { get; set; }

    [StringLength(1000)]
    public string? ResolutionNotes { get; set; }
}