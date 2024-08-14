namespace Wta.Application.Mes.Domain;

[BaseData, Display(Name = "物料", Order = 1)]
public class Material : Entity
{
}

[BaseData, Display(Name = "BOM", Order = 3)]
public class Bom : Entity
{
}

[Technology, Display(Name = "工艺", Order = 1)]
public class Technology : Entity
{
}
