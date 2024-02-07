using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Wta.Application.Identity.Domain;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Data;
using Wta.Infrastructure.Domain;
using Wta.Infrastructure.Extensions;
using Wta.Infrastructure.Interfaces;
using Wta.Shared;

namespace Wta.Application.Identity.Data;

public class IdentityDbSeeder(IActionDescriptorCollectionProvider actionProvider, IEncryptionService encryptionService, ITenantService tenantService) : IDbSeeder<IdentityDbContext>
{
    public void Seed(DbContext context)
    {
        //添加部门
        InitDepartment(context);
        //添加权限
        InitPermission(context);
        //添加角色
        InitRole(context);
        //添加用户
        InitUser(context);
    }

    private void InitUser(DbContext context)
    {
        var userName = "admin";
        var password = "123456";
        //if (tenantService.TenantId.HasValue)
        //{
        //    userName = tenantService.UserName!;
        //}
        var salt = encryptionService.CreateSalt();
        var passwordHash = encryptionService.HashPassword(password, salt);

        context.Set<User>().Add(new User
        {
            Name = "管理员",
            UserName = userName,
            Avatar = "api/upload/avatar.svg",
            NormalizedUserName = userName.ToUpperInvariant(),
            SecurityStamp = salt,
            PasswordHash = passwordHash,
            IsReadOnly = true,
            UserRoles = new List<UserRole> {
                new UserRole
                {
                    Role = context.Set<Role>().FirstOrDefault(o=>o.Number=="admin"),
                    IsReadOnly = true
                }
            }
        });
        context.SaveChanges();
    }

    private void InitRole(DbContext context)
    {
        //var permisions = tenantService.TenantId.HasValue ? tenantService.Permissions : context.Set<Permission>().Select(o => o.Id).ToList();
        var permisions = context.Set<Permission>().Select(o => o.Id).ToList();
        context.Set<Role>().Add(new Role
        {
            Name = "管理员",
            Number = "admin",
            RolePermissions = permisions!.Select(o => new RolePermission
            {
                PermissionId = o,
                IsReadOnly = true
            }).ToList()
        });
        context.SaveChanges();
    }

    private static void InitDepartment(DbContext context)
    {
        context.Set<Department>().Add(new Department
        {
            Name = "机构",
            Number = "Organ",
            Children = [
                    new Department
                    {
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
                Type = MenuType.Group,
                Authorize = "Authenticated",
                Number = "user-center",
                Name = "userCenter",
                Icon = "user",
                Order = 2,
                Children = [
                    new Permission
                    {
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
        WebApp.Instance.Assemblies
            .SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.IsAssignableTo(typeof(IResource)))
            .ForEach(resourceType =>
            {
                if (tenantService.TenantId.HasValue && resourceType == typeof(Tenant))
                {
                    return;
                }
                // 菜单
                var resourceServiceType = typeof(IResourceService<>).MakeGenericType(resourceType);
                var resourcePermission = new Permission
                {
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
                    var number = $"{descriptor.ControllerName}.{descriptor.ActionName}";
                    resourcePermission.Children.Add(new Permission
                    {
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
