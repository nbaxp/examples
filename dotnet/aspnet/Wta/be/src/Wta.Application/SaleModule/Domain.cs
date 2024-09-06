using Wta.Application.SystemModule.Data;

namespace Wta.Application.SaleModule;

[DependsOn<SystemDbContext>, SaleManagement, Display(Name = "报价单", Order = 10)]
public class QuotationOrder : Entity
{
}

[DependsOn<SystemDbContext>, SaleManagement, Display(Name = "销售订单", Order = 20)]
public class SaleOrder : Entity
{
}

[DependsOn<SystemDbContext>, SaleManagement, Display(Name = "销售出库", Order = 40)]
public class SaleDelivery : Entity
{
}

[DependsOn<SystemDbContext>, SaleManagement, Display(Name = "销售退货", Order = 50)]
public class SaleReturn : Entity
{
}

[DependsOn<SystemDbContext>, SaleManagement, Display(Name = "报价单统计", Order = 60)]
public class QuotationStatistics : Entity
{
}

[DependsOn<SystemDbContext>, SaleManagement, Display(Name = "销售订单统计", Order = 70)]
public class SaleOrderStatistics : Entity
{
}

[DependsOn<SystemDbContext>, SaleManagement, Display(Name = "销售执行跟踪看板", Order = 80)]
public class SalesExecutionTrackingBoard : Entity
{
}
