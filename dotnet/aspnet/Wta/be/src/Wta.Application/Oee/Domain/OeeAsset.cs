using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[Oee]
[DependsOn<PlatformDbContext>]
[Display(Name = "OEE资产", Order = 10)]
public class OeeAsset : BaseTreeEntity<OeeAsset>, IEntityTypeConfiguration<OeeAsset>
{
    [Hidden]
    public List<OeeShift> Shifts { get; set; } = [];

    [Hidden]
    public List<OeeData> Datas { get; set; } = [];

    public void Configure(EntityTypeBuilder<OeeAsset> builder)
    {
    }
}
