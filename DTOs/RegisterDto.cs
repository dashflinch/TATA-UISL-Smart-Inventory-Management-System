using System.ComponentModel.DataAnnotations;

namespace CompanyInventory.DTOs;

public class RegisterDto
{
    [Required]
    [Display(Name = "Employee Code")]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(Password))]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}