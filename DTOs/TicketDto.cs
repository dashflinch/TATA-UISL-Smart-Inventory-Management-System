using System.ComponentModel.DataAnnotations;

namespace CompanyInventory.DTOs;

public class TicketDto
{
    public int TicketId { get; set; }

    [Required]
    [StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string IssueCategory { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string IssueType { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string AffectedSystem { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Impact { get; set; } = string.Empty;

    // Optional Product Link
    public int? ProductId { get; set; }

    [Required]
    public string Priority { get; set; } = "Medium";

    public string Status { get; set; } = "Open";

    public string? AssignedTo { get; set; }

    [StringLength(1000)]
    public string? ResolutionNotes { get; set; }
}