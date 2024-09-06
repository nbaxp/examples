using Wta.Application.SystemModule.Data;

namespace Wta.Application.MesModule;

[MesBaseData, Display(Name = "物料", Order = 1)]
[DependsOn<SystemDbContext>]
public class Material : Entity
{
}

[MesBaseData, Display(Name = "BOM", Order = 3)]
[DependsOn<SystemDbContext>]
public class Bom : Entity
{
}

[Technology, Display(Name = "工艺", Order = 1)]
[DependsOn<SystemDbContext>]
public class Technology : Entity
{
}
