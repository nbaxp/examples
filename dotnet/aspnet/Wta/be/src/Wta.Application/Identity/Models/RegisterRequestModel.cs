using Microsoft.AspNetCore.Mvc;

namespace Wta.Application.Identity.Models;

public class RegisterModelBase : CaptchaModel
{
    [Required]
    [RegularExpression(@"\w{4,64}")]
    [Remote("ValidUserName", "User")]
    public string? UserName { get; set; }

    [Required]
    public string? Password { get; set; }

    [Compare("Password")]
    public string? ConfirmPassword { get; set; }
}

public class EmailRegisterModel : RegisterModelBase
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}

public class SmsRegisterModel : RegisterModelBase
{
    [Required]
    [Phone]
    public string? PhoneNumber { get; set; }
}
