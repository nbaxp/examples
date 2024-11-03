using Wta.Application.Platform.Data;

namespace Wta.Application.BaseData.Domain;

[DependsOn<PlatformDbContext>, BaseData, Display(Name = "产品分类", Order = 20)]
public class PruductCategory : Entity
{
}
