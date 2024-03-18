using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Wta.Application.Default.Domain;
using Wta.Infrastructure;
using Wta.Infrastructure.Application.Domain;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Auth;
using Wta.Infrastructure.Data;
using Wta.Infrastructure.Extensions;
using Wta.Infrastructure.Security;
using Wta.Infrastructure.Tenant;

namespace Wta.Application.Default.Data;

public class DefaultDbSeeder(IActionDescriptorCollectionProvider actionProvider, IEncryptionService encryptionService, ITenantService tenantService) : IDbSeeder<DefaultDbContext>
{
    public void Seed(DefaultDbContext context)
    {
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

    private static void InitDict(DbContext context)
    {
        context.Set<Dict>().Add(new Dict
        {
            Id = context.NewGuid(),
            Name = "语言",
            Number = "language",
            Children = new List<Dict>
            {
                new Dict
                {
                    Id= context.NewGuid(),
                    Name="简体中文",
                    Number="zh-CN"
                },
                new Dict
                {
                    Id= context.NewGuid(),
                    Name="English",
                    Number="en-US"
                }
            }
        }.UpdateNode());
    }

    private void InitUser(DbContext context)
    {
        var userName = "admin";
        var password = "123456";
        var salt = encryptionService.CreateSalt();
        var passwordHash = encryptionService.HashPassword(password, salt);

        var userId = context.NewGuid();

        context.Set<User>().Add(new User
        {
            Id = userId,
            Name = tenantService.TenantNumber != null ? "租户管理员" : "管理员",
            UserName = userName,
            Avatar = "api/file/avatar.svg",
            NormalizedUserName = userName.ToUpperInvariant(),
            SecurityStamp = salt,
            PasswordHash = passwordHash,
            IsReadOnly = true,
            UserRoles = new List<UserRole> {
                new UserRole
                {
                    UserId=userId,
                    RoleId = context.Set<Role>().First(o=>o.Number=="admin").Id,
                    IsReadOnly = true
                }
            }
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

    private void InitPermission(DbContext context)
    {
        var list = new List<Permission>
        {
            new Permission
            {
                Id = context.NewGuid(),
                Type = MenuType.Menu,
                Authorize = "Anonymous",
                Number = "home",
                Component = "home",
                Name = "home",
                Icon = "home",
                NoCache = true,
                Order = 1
            },
            new Permission
            {
                Id = context.NewGuid(),
                Type = MenuType.Group,
                Authorize = "Authenticated",
                Number = "user-center",
                Name = "userCenter",
                Icon = "user",
                Order = 2,
                Children = [
                    new Permission
                    {
                        Id = context.NewGuid(),
                        Type = MenuType.Menu,
                        Authorize = "Authenticated",
                        Number = "reset-asswrod",
                        Component = "reset-password",
                        Name = "resetPassword",
                        Order = 1
                    }
                ]
            }
        };
        var order = 1;
        var actionDescriptors = actionProvider.ActionDescriptors.Items;
        WtaApplication.Assemblies
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
                var resourcePermission = new Permission
                {
                    Id = context.NewGuid(),
                    Type = MenuType.Menu,
                    Authorize = "Authenticated",
                    Name = resourceType.Name.ToLowerCamelCase(),
                    Number = resourceType.Name.ToSlugify()!,
                    Component = "list",
                    Schema = $"{resourceType.Name.ToSlugify()}",
                    Order = resourceType.GetCustomAttribute<DisplayAttribute>()?.Order ?? order++
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
                    resourcePermission.Children.Add(new Permission
                    {
                        Id = context.NewGuid(),
                        Type = MenuType.Button,
                        Authorize = number,
                        Name = (descriptor.MethodInfo.GetCustomAttribute<DisplayAttribute>()?.Name ?? descriptor.ActionName).ToLowerCamelCase(),
                        Number = number,
                        Url = descriptor.AttributeRouteInfo?.Template,
                        Method = (descriptor.ActionConstraints?.FirstOrDefault() as HttpMethodActionConstraint)?.HttpMethods.FirstOrDefault(),
                        Command = descriptor.ActionName.ToSlugify(),
                        ButtonType = descriptor.MethodInfo.GetCustomAttribute<ButtonAttribute>()?.Type ?? ButtonType.Table,
                        Hidden = descriptor.MethodInfo.GetCustomAttribute<HiddenAttribute>() == null ? false : true,
                        Order = descriptor.MethodInfo.GetCustomAttribute<DisplayAttribute>()?.Order ?? 0
                    });
                });
                // 分组
                var groupAttribute = resourceType.GetCustomAttributes().FirstOrDefault(o => o.GetType().IsAssignableTo(typeof(GroupAttribute)));
                if (groupAttribute != null && groupAttribute is GroupAttribute group)
                {
                    var groupPermission = list.FirstOrDefault(o => o.Number == group.Name.ToSlugify());
                    if (groupPermission == null)
                    {
                        groupPermission = new Permission
                        {
                            Id = context.NewGuid(),
                            Type = MenuType.Group,
                            Authorize = "Anonymous",
                            Name = group.Name.ToLowerCamelCase(),
                            Number = group.Name.ToSlugify()!,
                            Icon = group.Icon,
                            Order = group.Order
                        };
                        list.Add(groupPermission);
                    }
                    groupPermission.Children.Add(resourcePermission);
                }
                else
                {
                    list.Add(resourcePermission);
                }
            });
        list.ForEach(o =>
        {
            o.UpdateNode();
            context.Set<Permission>().Add(o);
        });
        context.SaveChanges();
    }
}
