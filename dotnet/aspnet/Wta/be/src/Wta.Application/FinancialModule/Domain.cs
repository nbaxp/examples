using Wta.Application.Platform.Data;

namespace Wta.Application.FinancialModule;

[DependsOn<PlatformDbContext>, ReceivablePayabl, Display(Name = "销售出库应收账款", Order = 10)]
public class SalesOutboundReceivable : Entity
{
}

[DependsOn<PlatformDbContext>, ReceivablePayabl, Display(Name = "销售退货应收账款", Order = 20)]
public class SalesReturnReceivable : Entity
{
}

[DependsOn<PlatformDbContext>, ReceivablePayabl, Display(Name = "采购入库应付账款", Order = 30)]
public class PurchaseStoragePayable : Entity
{
}

[DependsOn<PlatformDbContext>, ReceivablePayabl, Display(Name = "采购退货应付账款", Order = 40)]
public class PurchaseReturnPayable : Entity
{
}

[DependsOn<PlatformDbContext>, Financial, Display(Name = "应收账款对账", Order = 10)]
public class ReceivableAccountChecking : Entity
{
}

[DependsOn<PlatformDbContext>, Financial, Display(Name = "销项发票", Order = 20)]
public class SalesInvoice : Entity
{
}

[DependsOn<PlatformDbContext>, Financial, Display(Name = "收款单", Order = 30)]
public class PaymentReceipt : Entity
{
}

[DependsOn<PlatformDbContext>, Financial, Display(Name = "应收账款统计", Order = 40)]
public class AccountsReceivableStatistics : Entity
{
}

[DependsOn<PlatformDbContext>, Financial, Display(Name = "应付账款对账", Order = 50)]
public class AccountsPayableChecking : Entity
{
}

[DependsOn<PlatformDbContext>, Financial, Display(Name = "进项发票", Order = 60)]
public class InputInvoice : Entity
{
}

[DependsOn<PlatformDbContext>, Financial, Display(Name = "付款单", Order = 70)]
public class PaymentOrder : Entity
{
}

[DependsOn<PlatformDbContext>, Financial, Display(Name = "应付账款统计", Order = 80)]
public class AccountsPayableStatistics : Entity
{
}
