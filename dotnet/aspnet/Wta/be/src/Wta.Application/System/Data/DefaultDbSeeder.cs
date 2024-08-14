using Wta.Infrastructure.Scheduling;

namespace Wta.Application.System.Data;

[Display(Order = -1)]
public class DefaultDbSeeder(IActionDescriptorCollectionProvider actionProvider, IEncryptionService encryptionService, ITenantService tenantService) : IDbSeeder<DefaultDbContext>
{
    public void Seed(DefaultDbContext context)
    {
        //添加定时任务
        InitJob(context);
        //添加字典
        InitDict(context);
        //添加部门
        InitDepartment(context);
        //添加权限
        InitPermission(context);
        //添加角色
        InitRole(context);
        //添加用户
        InitUser(context);
    }

    private static void InitJob(DefaultDbContext context)
    {
        AppDomain.CurrentDomain.GetCustomerAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(o => o.IsClass && !o.IsAbstract && o.IsAssignableTo(typeof(IScheduledTask)) && o.HasAttribute<CronAttribute>())
            .ForEach(o =>
            {
                context.Set<Job>().Add(new Job { Name = o.GetDisplayName(), Type = o.FullName!, Cron = o.GetCustomAttribute<CronAttribute>()!.Cron });
            });
        context.SaveChanges();
    }

    private static void InitDepartment(DbContext context)
    {
        context.Set<Department>().Add(new Department
        {
            Id = context.NewGuid(),
            Name = "机构",
            Number = "Organ",
            Children = [
                    new Department
                    {
                        Id = context.NewGuid(),
                        Name = "技术部",
                        Number = "Technology"
                    }
                ]
        }.UpdateNode());
        context.SaveChanges();
    }

    private static void InitDict(DbContext context)
    {
        context.Set<Dict>().Add(new Dict
        {
            Id = context.NewGuid(),
            Name = "语言",
            Number = "language",
            Children =
            [
                new() {
                    Id= context.NewGuid(),
                    Name="简体中文",
                    Number="zh-CN"
                },
                new() {
                    Id= context.NewGuid(),
                    Name="English",
                    Number="en-US"
                }
            ]
        }.UpdateNode());
    }

    private void InitPermission(DbContext context)
    {
        var list = new List<Permission>();
        // 添加菜单分组
        var groups = AppDomain.CurrentDomain.GetCustomerAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(o => o.IsClass && !o.IsAbstract && o.IsAssignableTo(typeof(GroupAttribute)))
            .ToList();
        /// 添加菜单分组
        groups.ForEach(groupType =>
        {
            var number = groupType.Name.ToSlugify()!;
            if (!list.Any(o => o.Number == number))
            {
                var group = new Permission
                {
                    Id = context.NewGuid(),
                    Type = MenuType.Group,
                    Authorize = "Anonymous",
                    Name = groupType.GetDisplayName(),
                    Number = groupType.FullName?.TrimEnd("Attribute")!,
                    RoutePath = groupType.Name.TrimEnd("Attribute").ToSlugify()!,
                    Redirect = groupType.GetCustomAttributes<KeyValueAttribute>(false).FirstOrDefault(o => o.Key == "Redirect")?.Value.ToString(),
                    Icon = groupType.GetCustomAttribute<IconAttribute>()?.Icon ?? "folder",
                    Component = groupType.GetCustomAttribute<ViewAttribute>()?.Component,
                    Order = groupType.GetCustomAttribute<DisplayAttribute>()?.GetOrder() ?? 0
                };
                list.Add(group);
            }
        });
        /// 设置分组上级

        groups.ForEach(groupType =>
        {
            var number = groupType.FullName?.TrimEnd("Attribute")!;
            var group = list.FirstOrDefault(o => o.Number == number)!;
            if (groupType.BaseType != null && !groupType.BaseType.IsAbstract)
            {
                var parentNumber = groupType.BaseType!.FullName!.TrimEnd("Attribute")!;
                group.ParentId = list.FirstOrDefault(o => o.Number == parentNumber)?.Id;
            }
        });

        //添加资源菜单和资源操作按钮
        var order = 1;
        var actionDescriptors = actionProvider.ActionDescriptors.Items;
        AppDomain.CurrentDomain.GetCustomerAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.IsAssignableTo(typeof(IResource)))
            .ForEach(resourceType =>
            {
                if (tenantService.TenantNumber != null && resourceType == typeof(Tenant))
                {
                    return;
                }
                // 菜单
                var resourceServiceType = typeof(IResourceService<>).MakeGenericType(resourceType);
                var controllerType = actionDescriptors.Cast<ControllerActionDescriptor>()
                .FirstOrDefault(o => o.ControllerTypeInfo.AsType().IsAssignableTo(resourceServiceType))?
                .ControllerTypeInfo.AsType()!;
                var component = resourceType.GetCustomAttribute<ViewAttribute>()?.Component ?? controllerType.GetCustomAttribute<ViewAttribute>()?.Component ?? "_list";
                var resourcePermission = new Permission
                {
                    Id = context.NewGuid(),
                    Type = MenuType.Menu,
                    Authorize = "Authenticated",
                    Name = resourceType.GetDisplayName(),
                    Number = resourceType.FullName!,
                    RoutePath = resourceType.Name.TrimEnd("Model").ToSlugify()!,
                    Component = component,
                    NoCache = controllerType.GetCustomAttribute<NoCacheAttribute>()?.NoCache ?? false,
                    //Schema = $"{resourceType.Name.ToSlugify()}",
                    Icon = controllerType.GetCustomAttribute<IconAttribute>()?.Icon ?? "file",
                    Order = resourceType.GetCustomAttribute<DisplayAttribute>()?.GetOrder() ?? order++
                };
                // 按钮
                actionDescriptors
                .Select(o => (o as ControllerActionDescriptor)!)
                .Where(o => o != null && o.ControllerTypeInfo.AsType().IsAssignableTo(resourceServiceType) && !o.MethodInfo.GetCustomAttributes<IgnoreAttribute>().Any())
                .ForEach(descriptor =>
                {
                    if (descriptor.ControllerTypeInfo.AsType().GetCustomAttribute<ViewAttribute>()?.Component is string component)
                    {
                        resourcePermission.Component = component;
                    }
                    var number = $"{descriptor.ControllerName}.{descriptor.ActionName}";

                    list.Add(new Permission
                    {
                        ParentId = resourcePermission.Id,
                        Id = context.NewGuid(),
                        Type = MenuType.Button,
                        Authorize = number,
                        Name = (descriptor.MethodInfo.GetCustomAttribute<DisplayAttribute>()?.Name ?? descriptor.ActionName).ToLowerCamelCase(),
                        Number = number,
                        RoutePath = number,
                        Url = descriptor.AttributeRouteInfo?.Template,
                        Method = (descriptor.ActionConstraints?.FirstOrDefault() as HttpMethodActionConstraint)?.HttpMethods.FirstOrDefault(),
                        Command = descriptor.ActionName.ToSlugify(),
                        ButtonType = descriptor.MethodInfo.GetCustomAttribute<ButtonAttribute>()?.Type ?? ButtonType.Table,
                        Hidden = descriptor.MethodInfo.GetCustomAttribute<HiddenAttribute>() != null,
                        Order = descriptor.MethodInfo.GetCustomAttribute<DisplayAttribute>()?.GetOrder() ?? 0
                    });
                });
                // 设置菜单分组
                var groupAttribute = resourceType.GetCustomAttributes().FirstOrDefault(o => o.GetType().IsAssignableTo(typeof(GroupAttribute)));
                if (groupAttribute != null && groupAttribute is GroupAttribute group)
                {
                    var number = group.GetType().FullName?.TrimEnd("Attribute")!;
                    var groupPermission = list.FirstOrDefault(o => o.Number == number);
                    if (groupPermission != null)
                    {
                        resourcePermission.ParentId = groupPermission.Id;
                    }
                }
                list.Add(resourcePermission);
            });
        list.AsQueryable()
            .Cast<BaseTreeEntity<Permission>>()
            .ToList()
            .ToTree()
            .Cast<Permission>()
            .ForEach(o =>
            {
                o.UpdateNode();
                context.Set<Permission>().Add(o);
            });
        context.SaveChanges();
    }

    private void InitRole(DbContext context)
    {
        var permisions = context.Set<Permission>().ToList();
        if (tenantService.TenantNumber != null)
        {
            permisions.Where(o => !o.Disabled && !tenantService.Permissions.Contains(o.Number)).ForEach(o => o.Disabled = true);
            permisions = permisions.Where(o => tenantService.Permissions.Contains(o.Number)).ToList();
        }
        context.Set<Role>().Add(new Role
        {
            Id = context.NewGuid(),
            Name = tenantService.TenantNumber != null ? "租户管理员" : "管理员",
            Number = "admin",
            RolePermissions = permisions!.Select(o => new RolePermission
            {
                PermissionId = o.Id,
                IsReadOnly = true
            }).ToList()
        });
        context.Set<Role>().Add(new Role
        {
            Id = context.NewGuid(),
            Name = "测试",
            Number = "test",
        });
        context.SaveChanges();
    }

    private void InitUser(DbContext context)
    {
        var userName = "admin";
        var password = "123456";
        var salt = encryptionService.CreateSalt();
        var passwordHash = encryptionService.HashPassword(password, salt);
        var userId = context.NewGuid();
        var email = "76527413@qq.com";

        context.Set<User>().Add(new User
        {
            Id = userId,
            UserName = userName,
            Email = email,
            NormalizedEmail = email.ToUpperInvariant(),
            EmailConfirmed = true,
            Name = tenantService.TenantNumber != null ? "租户管理员" : "管理员",
            Avatar = "api/file/avatar.svg",
            NormalizedUserName = userName.ToUpperInvariant(),
            SecurityStamp = salt,
            PasswordHash = passwordHash,
            IsReadOnly = true,
            UserRoles = [
                new() {
                    UserId=userId,
                    RoleId = context.Set<Role>().First(o=>o.Number=="admin").Id,
                    IsReadOnly = true
                }
            ],
            DepartmentId = context.Set<Department>().FirstOrDefault()?.Id
        });
        context.SaveChanges();
    }
}
