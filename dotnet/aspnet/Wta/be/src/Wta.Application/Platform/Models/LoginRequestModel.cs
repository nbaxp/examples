namespace Wta.Application.Platform.Models;

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
    [Required]
    [Display(Name ="租户")]
    public string TenantNumber { get; set; } = default!;

    [KeyValue("icon", "ep-user")]
    public string UserName { get; set; } = default!;

    [Required]
    [DataType(DataType.Password)]
    [KeyValue("icon", "ep-lock")]
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
