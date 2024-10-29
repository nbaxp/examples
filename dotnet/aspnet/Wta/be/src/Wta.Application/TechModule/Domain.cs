using Wta.Application.Platform.Data;

namespace Wta.Application.TechModule;

[DependsOn<PlatformDbContext>, TechManagement, Display(Name = "方案设计", Order = 10)]
public class SchemeDesign : Entity
{
}

[DependsOn<PlatformDbContext>, TechManagement, Display(Name = "产品信息", Order = 20)]
public class ProductInfo : Entity
{
}

[DependsOn<PlatformDbContext>, TechManagement, Display(Name = "产品BOM", Order = 30)]
public class ProductBom : Entity
{
}

[DependsOn<PlatformDbContext>, TechManagement, Display(Name = "生产工序", Order = 40)]
public class PrudoctionProcess : Entity
{
}

[DependsOn<PlatformDbContext>, TechManagement, Display(Name = "产品列表", Order = 50)]
public class ProductList : Entity
{
}

[DependsOn<PlatformDbContext>, TechManagement, Display(Name = "生产班组", Order = 60)]
public class PruductionTeam : Entity
{
}
