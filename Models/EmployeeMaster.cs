using System.ComponentModel.DataAnnotations;

namespace CompanyInventory.Models
{
    public class EmployeeMaster
    {
        [Key]
        public int EmployeeMasterId { get; set; }

        [Required]
        public string EmployeeCode { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Department { get; set; }

        [Required]
        public string Role { get; set; }

        public bool IsRegistered { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime? UpdatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }
    }
}