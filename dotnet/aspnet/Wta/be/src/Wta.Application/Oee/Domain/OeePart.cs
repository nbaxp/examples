using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[Oee]
[DependsOn<PlatformDbContext>]
[Display(Name = "OEE零件", Order = 30)]
public class OeePart : BaseNameNumberEntity
{
    [Display(Name = "OP_CODE")]
    public string OpCode { get; set; } = default!;

    [Display(Name = "标准速率")]
    public float StandardUpm { get; set; } = 0f;
}
