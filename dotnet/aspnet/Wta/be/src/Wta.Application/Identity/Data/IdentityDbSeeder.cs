using System.Reflection;
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

public class IdentityDbSeeder(IActionDescriptorCollectionProvider actionProvider, IEncryptionService passwordHasher) : IDbSeeder<IdentityDbContext>
{
    public void Seed(IdentityDbContext context)
    {
        //添加部门
        var departments = InitDepartment(context);
        //添加权限
        var permissions = InitPermission(context);
        //添加管理员角色
        var adminRole = new Role
        {
            Name = "管理员",
            Number = "admin",
            RolePermissions = permissions.Select(o => new RolePermission
            {
                Permission = o,
                IsReadOnly = true
            }
            ).ToList()
        };
        context.Set<Role>().Add(adminRole);
        //添加管理员用户
        var userName = "admin";
        var password = "123456";
        var passwordHash = passwordHasher.HashPassword(password, userName);
        var admin = new User
        {
            Name = "管理员",
            UserName = userName,
            Avatar = "api/upload/avatar.svg",
            NormalizedUserName = userName.ToUpperInvariant(),
            SecurityStamp = userName,
            PasswordHash = passwordHash,
            IsReadOnly = true,
            UserRoles = new List<UserRole> {
                new UserRole
                {
                    Role = adminRole,
                    IsReadOnly = true
                }
            }
        };
        context.Set<User>().Add(admin);
    }

    private static List<Department> InitDepartment(IdentityDbContext context)
    {
        var list = new List<Department> {
            new Department{
                Name="机构",
                Number="Organ",
                Children=new List<Department>{
                    new Department
                    {
                        Name="技术部",
                        Number="Technology"
                    }
                }
            }
        };
        foreach (var item in list)
        {
            context.Set<Department>().Add(item);
        }
        return list;
    }

    private List<Permission> InitPermission(IdentityDbContext context)
    {
        var order = 1;
        var list = new List<Permission>
        {
            new Permission
            {
                Type = MenuType.Menu,
                Authorize="Authenticated",
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
                Authorize="Authenticated",
                Number = "user-center",
                Name = "userCenter",
                Icon = "user",
                Order = 2,
                Children = new List<Permission> {
                    new Permission {
                        Type = MenuType.Menu,
                        Authorize="Authenticated",
                        Number = "reset-asswrod",
                        Component = "reset-password",
                        Name = "resetPassword",
                        Order = 1
                    }
                }
            }
        };
        var actionDescriptors = actionProvider.ActionDescriptors.Items;
        WebApp.Instance.Assemblies
            .SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.IsAssignableTo(typeof(IResource)))
            .ForEach(resourceType =>
            {
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
                list.Add(resourcePermission);
                // 按钮
                actionDescriptors
                .Select(o => (o as ControllerActionDescriptor)!)
                .Where(o => o != null && o.ControllerTypeInfo.AsType().IsAssignableTo(resourceServiceType) && o.MethodInfo.GetCustomAttribute<IgnoreAttribute>() == null)
                .ForEach(descriptor =>
                {
                    var number = $"{descriptor.ControllerName}.{descriptor.ActionName}";
                    list.Add(new Permission
                    {
                        ParentId = resourcePermission.Id,
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
                    resourcePermission.ParentId = groupPermission.Id;
                }
            });
        context.Set<Permission>().AddRange(list);
        return list;
    }
}
