namespace Wta.Application.Default.Models;

[UserCenter]
[Display(Name = "修改密码", Order = 2)]
[KeyValue("url", "reset-password/index")]

public class ResetPasswordModel : IResource
{
    [Required]
    [DataType(DataType.Password)]
    public string? CurrentPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [StringLength(24, MinimumLength = 6)]
    public string? NewPassword { get; set; }

    [Compare(nameof(NewPassword))]
    [DataType(DataType.Password)]
    public string? ConfirmNewPassword { get; set; }
}
