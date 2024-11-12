using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[OeeBaseData]
[DependsOn<PlatformDbContext>]
[Display(Name = "资产", Order = 10)]
public class OeeAsset : BaseTreeEntity<OeeAsset>, IEntityTypeConfiguration<OeeAsset>
{
    [Hidden]
    public List<OeeShift> Shifts { get; set; } = [];

    [Hidden]
    public List<OeeRange> Ranges { get; set; } = [];

    public void Configure(EntityTypeBuilder<OeeAsset> builder)
    {
    }
}
