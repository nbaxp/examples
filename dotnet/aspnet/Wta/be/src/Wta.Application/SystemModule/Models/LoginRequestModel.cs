namespace Wta.Application.SystemModule.Models;

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

    [Hidden]
    public string? client_id { get; set; }

    [Hidden]
    public string? return_to { get; set; }

    [Hidden]
    public string? anti_token { get; set; }

    [Hidden]
    public string? provider { get; set; }

    [Hidden]
    public string? open_id { get; set; }

    [Hidden]
    public string? access_token { get; set; }
}
