namespace Wta.Application.Platform.Domain;

[DependsOn<PlatformDbContext>, SystemSettings, Display(Name = "应用", Order = 8)]
public class ExternalApp : Entity, IEntityTypeConfiguration<ExternalApp>
{
    [Hidden]
    public Guid UserId { get; set; }

    [Display(Name = "应用名称")]
    public string Name { get; set; } = null!;

    [Display(Name = "应用简介")]
    public string Description { get; set; } = null!;

    [UIHint("image-upload")]
    [KeyValue("accept", ".svg,.png")]
    [KeyValue("url", "file/upload")]
    [Display(Name = "应用图标")]
    [KeyValue("hideForQuery", true)]
    public string Icon { get; set; } = null!;

    [Display(Name = "应用首页")]
    public string Home { get; set; } = null!;

    [Display(Name = "应用回调地址")]
    public string Callback { get; set; } = null!;

    [Display(Name = "应用注销地址")]
    public string? Logout { get; set; }

    [Display(Name = "Client Id")]
    [ReadOnly(true)]
    public string ClientId { get; set; } = null!;

    [Display(Name = "Client Secret")]
    [ReadOnly(true)]
    [KeyValue("hideForQuery", true)]
    [DataType(DataType.Password)]
    public string ClientSecret { get; set; } = null!;

    [Display(Name = "禁用")]
    public bool Disabled { get; set; }

    [Hidden]
    public User? User { get; set; }

    public void Configure(EntityTypeBuilder<ExternalApp> builder)
    {
        builder.HasIndex(o => new { o.TenantNumber, o.Name }).IsUnique();
        builder.HasOne(o => o.User).WithMany(o => o.Apps).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
