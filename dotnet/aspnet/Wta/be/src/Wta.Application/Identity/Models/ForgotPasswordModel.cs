namespace Wta.Application.Identity.Models;

public class ForgotPasswordModel : CaptchaModel
{
    [Required]
    [RegularExpression(@"^(\d{11}|\w+@、w+\.\w)$")]
    public string? EmailOrPhoneNumber { get; set; }

    [Required]
    public string? Password { get; set; }

    [Compare("Password")]
    public string? ConfirmPassword { get; set; }
}
