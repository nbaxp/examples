namespace Wta.Application.System.Domain;

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
