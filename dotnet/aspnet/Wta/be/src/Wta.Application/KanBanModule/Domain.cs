using Wta.Application.Platform;

namespace Wta.Application.KanBanModule;

[DependsOn<PlatformDbContext>, KanBan, Display(Name = "看板首页", Order = 10)]
public class KanbanHomepage : Entity
{
}

[DependsOn<PlatformDbContext>, KanBan, Display(Name = "生产统计看板", Order = 20)]
public class ProductionStatisticsKanban : Entity
{
}

[DependsOn<PlatformDbContext>, KanBan, Display(Name = "库存统计看板", Order = 30)]
public class InventoryStatisticsKanban : Entity
{
}

[DependsOn<PlatformDbContext>, KanBan, Display(Name = "仓库流水看板", Order = 40)]
public class WarehouseTransactionKanban : Entity
{
}

[DependsOn<PlatformDbContext>, KanBan, Display(Name = "销售订单看板", Order = 50)]
public class SalesOrderKanban : Entity
{
}

[DependsOn<PlatformDbContext>, KanBan, Display(Name = "采购定单看板", Order = 60)]
public class PurchaseOrderKanban : Entity
{
}

[DependsOn<PlatformDbContext>, KanBan, Display(Name = "财务收支看板", Order = 70)]
public class FinancialRevenueExpenseKanban : Entity
{
}
