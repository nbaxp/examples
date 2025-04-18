namespace Wta.Application.Platform.Models;

[UserCenter, Display(Name = "修改密码", Order = 30)]
[KeyValue("url", "reset-password/index")]
public class ResetPasswordModel : IResource
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "当前密码")]
    public string? CurrentPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [StringLength(24, MinimumLength = 6)]
    [Display(Name = "新密码")]
    public string? NewPassword { get; set; }

    [Compare(nameof(NewPassword), ErrorMessage = "新密码和确认新密码必须相同")]
    [DataType(DataType.Password)]
    [Display(Name = "确认新密码")]
    public string? ConfirmNewPassword { get; set; }
}
