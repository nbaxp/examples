using Wta.Application.SystemModule.Data;

namespace Wta.Application.ProcurementModule;

[DependsOn<SystemDbContext>, SupplierManagement, Display(Name = "供应商信息", Order = 10)]
public class SupplierInfo : Entity
{
}

[DependsOn<SystemDbContext>, SupplierManagement, Display(Name = "供应商价格表", Order = 20)]
public class SupplierPriceList : Entity
{
}

[DependsOn<SystemDbContext>, SupplierManagement, Display(Name = "供应商信息查询", Order = 30)]
public class SupplierInfoQuery : Entity
{
}

[DependsOn<SystemDbContext>, PurchaseManagement, Display(Name = "采购申请", Order = 10)]
public class PurchaseRequest : Entity
{
}

[DependsOn<SystemDbContext>, PurchaseManagement, Display(Name = "采购订单", Order = 20)]
public class PurchaseOrder : Entity
{
}

[DependsOn<SystemDbContext>, PurchaseManagement, Display(Name = "采购入库", Order = 30)]
public class PurchaseStorage : Entity
{
}

[DependsOn<SystemDbContext>, PurchaseManagement, Display(Name = "采购退货", Order = 40)]
public class PurchaseReturn : Entity
{
}

[DependsOn<SystemDbContext>, PurchaseManagement, Display(Name = "采购需求统计", Order = 50)]
public class PurchaseDemandStatistics : Entity
{
}

[DependsOn<SystemDbContext>, PurchaseManagement, Display(Name = "采购订单统计", Order = 60)]
public class PurchaseOrderStatistics : Entity
{
}

[DependsOn<SystemDbContext>, PurchaseManagement, Display(Name = "采购执行跟踪看板", Order = 70)]
public class PurchaseExecutionTrackingBoard : Entity
{
}
