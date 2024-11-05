namespace Wta.Application.Platform.Domain;

[SystemSettings, Display(Name = "登录服务", Order = 8)]
[DependsOn<PlatformDbContext>]
public class LoginProvider : Entity
{
    [Display(Name = "名称")]
    public string Name { get; set; } = default!;

    [Display(Name = "编号")]
    public string Number { get; set; } = default!;

    [Display(Name = "图标")]
    [UIHint("image-upload")]
    [KeyValue("accept", ".svg,.png")]
    [KeyValue("url", "file/upload")]
    [KeyValue("hideForQuery", true)] public string Icon { get; set; } = default!;

    public string ClientId { get; set; } = null!;

    public string ClientSecret { get; set; } = null!;

    public string? UserIdName { get; set; }

    public string CallbackPath { get; set; } = null!;

    [Display(Name = "Authorization 地址")]
    public string AuthorizationEndpoint { get; set; } = null!;

    public string TokenEndpoint { get; set; } = null!;

    public string UserInformationEndpoint { get; set; } = null!;
    public string UserInformationRequestMethod { get; set; } = null!;
    public string UserInformationTokenPosition { get; set; } = null!;

    [Display(Name = "禁用")]
    public bool Disabled { get; set; }
}
