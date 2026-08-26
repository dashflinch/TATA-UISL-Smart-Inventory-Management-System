namespace CompanyInventory.Models;

public abstract class BaseEntity
{
    public bool IsActive { get; set; } = true;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public DateTime? UpdatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}