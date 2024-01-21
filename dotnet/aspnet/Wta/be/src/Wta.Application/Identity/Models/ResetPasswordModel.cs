namespace Wta.Application.Identity.Models;

public class ResetPasswordModel
{
    [Required]
    [DataType(DataType.Password)]
    public string? OldPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [StringLength(24, MinimumLength = 8)]
    public string? Password { get; set; }

    [Compare(nameof(Password))]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }
}
