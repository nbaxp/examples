namespace Wta.Application;

#region 用户中心
[Display(Name = "用户中心", Order = 0)]
public class UserCenterAttribute : GroupAttribute
{
}
#endregion

#region 系统管理
[Display(Name = "系统管理", Order = 5)]
public class SystemManagementAttribute : GroupAttribute
{
}

[Display(Name = "组织管理", Order = 1)]
public class OrganizationManagementAttribute : SystemManagementAttribute
{
}

[Display(Name = "权限管理", Order = 2)]
public class PermissionManagementAttribute : SystemManagementAttribute
{
}

[Display(Name = "系统设置", Order = 3)]
public class SystemSettingsAttribute : SystemManagementAttribute
{
}
#endregion

#region 基础数据
[Display(Name = "基础数据", Order = 10)]
public class BaseDataAttribute : GroupAttribute
{
}
#endregion

#region 技术管理
[Display(Name = "技术管理", Order = 20)]
public class TechManagementAttribute : GroupAttribute
{
}

#endregion

#region 客户管理
[Display(Name = "客户管理", Order = 30)]
public class CustomerManagementAttribute : GroupAttribute
{
}
#endregion
#region 销售管理
[Display(Name = "销售管理", Order = 40)]
public class SaleManagementAttribute : GroupAttribute
{
}
#endregion

#region 生产管理
[Display(Name = "生产管理", Order = 50)]
public class MesAttribute : GroupAttribute
{
}

//[Display(Name = "基础数据", Order = 1)]
//public class MesBaseDataAttribute : MesAttribute
//{
//}

//[Display(Name = "工艺设计", Order = 2)]
//public class TechnologyAttribute : MesAttribute
//{
//}

#endregion

#region 采购管理
[Display(Name = "采购管理", Order = 60)]
public class PurchaseManagementAttribute : GroupAttribute
{
}

[Display(Name = "供应商管理", Order = 10)]
public class SupplierManagementAttribute : PurchaseManagementAttribute
{
}
#endregion

#region 库存管理
[Display(Name = "库存管理", Order = 70)]
public class WmsAttribute : GroupAttribute
{
}

[Display(Name = "仓库管理", Order = 3)]
public class WarehouseManagementAttribute : WmsAttribute
{
}
#endregion

#region 财务管理
[Display(Name = "财务管理", Order = 80)]
public class FinancialAttribute : GroupAttribute
{
}

[Display(Name = "应收应付明细", Order = 10)]
public class ReceivablePayablAttribute : FinancialAttribute
{
}

#endregion

#region 经营看板
[Display(Name = "经营看板", Order = 90)]
public class KanBanAttribute : GroupAttribute
{
}
#endregion
