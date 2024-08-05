namespace Wta.Application.Default.Models;

[Display(Name = "登录")]
[KeyValue("url", "token/create")]
[KeyValue("input", "form")]
[KeyValue("submitStyle", "width:100%")]
[KeyValue("labelWidth", 0)]
public class LoginRequestModel : CaptchaModel
{
    [UIHint("select")]
    [KeyValue("url", "tenant/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    public string? TenantNumber { get; set; }

    public string UserName { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [KeyValue("showLabel", true)]
    public bool RememberMe { get; set; }
}
