namespace Wta.Application.Platform.Models;

[Display(Name = "注册")]
[KeyValue("url", "user/register")]
[KeyValue("input", "form")]
[KeyValue("submitStyle", "width:100%")]
[KeyValue("labelWidth", 0)]
public class RegisterModel : CaptchaModel
{
    [Required]
    [RegularExpression(@"\w{4,64}")]
    [Remote("IsUserNameAvailable", "User")]
    [KeyValue("remote", "user/no-user")]
    [Display(Name = "用户名")]
    public string? UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "密码")]
    public string? Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password")]
    [Display(Name = "确认密码")]
    public string? ConfirmPassword { get; set; }

    [Display(Name = "邮箱")]
    public string? Email { get; set; }

    [Display(Name = "手机号")]
    public string PhoneNumber { get; set; } = null!;

    [Hidden]
    public string? provider { get; set; }

    [Hidden]
    public string? open_id { get; set; }

    //[Required]
    //[RegularExpression(@"^(1\d{10}|\w+@\w+\.\w+)$")]
    //[UIHint("code-captcha")]
    //[KeyValue("url", "captcha/code")]
    //[Display(Name = "邮箱或手机号")]
    //public string? EmailOrPhoneNumber { get; set; }
}
