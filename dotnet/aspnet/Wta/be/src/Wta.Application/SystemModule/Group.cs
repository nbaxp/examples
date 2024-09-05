namespace Wta.Application.SystemModule;

[Display(Name = "系统管理", Order = 1000), KeyValue("Redirect", "/system-management/monitor")]
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

[Display(Name = "用户中心", Order = -10), KeyValue("Redirect", "/user-center/user-info")]
public class UserCenterAttribute : GroupAttribute
{
}
