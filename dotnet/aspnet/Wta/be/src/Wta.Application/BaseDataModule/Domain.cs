using Wta.Application.SystemModule.Data;

namespace Wta.Application.BaseDataModule;

[DependsOn<SystemDbContext>, BaseData, Display(Name = "产品单位", Order = 10)]
public class PruductUnit : Entity
{
}

[DependsOn<SystemDbContext>, BaseData, Display(Name = "产品分类", Order = 20)]
public class PruductCategory : Entity
{
}
