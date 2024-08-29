namespace Wta.Application.MesModule.Domain;

[MesBaseData, Display(Name = "物料", Order = 1)]
public class Material : Entity
{
}

[MesBaseData, Display(Name = "BOM", Order = 3)]
public class Bom : Entity
{
}

[Technology, Display(Name = "工艺", Order = 1)]
public class Technology : Entity
{
}
