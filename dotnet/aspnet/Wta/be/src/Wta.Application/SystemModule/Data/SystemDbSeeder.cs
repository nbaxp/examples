using ClosedXML;
using Wta.Application.SystemModule.Domain;
using Wta.Infrastructure.Scheduling;

namespace Wta.Application.SystemModule.Data;

[Display(Order = -1)]
public class SystemDbSeeder(IActionDescriptorCollectionProvider actionProvider, IEncryptionService encryptionService, ITenantService tenantService) : IDbSeeder<SystemDbContext>
{
    public void Seed(SystemDbContext context)
    {
        //添加定时任务
        InitJob(context);
        //添加字典
        InitDict(context);
        //添加部门
        InitDepartment(context);
        //添加权限
        var permissions = InitPermission(context);
        //添加角色
        var roles = InitRole(context, permissions);
        //添加岗位
        InitPost(context);
        //添加用户
        InitUser(context, roles);
        //保存
        context.SaveChanges();
    }

    private static void InitJob(SystemDbContext context)
    {
        AppDomain.CurrentDomain.GetCustomerAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(o => o.IsClass && !o.IsAbstract && o.IsAssignableTo(typeof(IScheduledTask)) && o.HasAttribute<CronAttribute>())
            .ForEach(o =>
            {
                context.Set<Job>().Add(new Job { Name = o.GetDisplayName(), Type = o.FullName!, Cron = o.GetCustomAttribute<CronAttribute>()!.Cron });
            });
    }

    private static void InitDepartment(DbContext context)
    {
        var department = new Department
        {
            Id = context.NewGuid(),
            Name = "工厂",
            Number = "0000",
            Children = [
                new()
                {
                    Id = context.NewGuid(),
                    Name = "市场营销中心",
                    Number = "0100",
                    Children = [
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "市场部",
                            Number = "0101",
                        },
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "销售部",
                            Number = "0102",
                        },
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "电子商务部",
                            Number = "0103",
                        },
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "售前支持部",
                            Number = "0104",
                        },
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "售后支持部",
                            Number = "0105",
                        }
                    ]
                },
                new()
                {
                    Id = context.NewGuid(),
                    Name = "研发生产中心",
                    Number = "0200",
                    Children = [
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "质量部",
                            Number = "0201",
                        },
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "计划部",
                            Number = "0202",
                        },
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "工程部",
                            Number = "0203",
                        },
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "研发部",
                            Number = "0204",
                        },
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "第一车间",
                            Number = "0205",
                        },
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "第二车间",
                            Number = "0206",
                        }
                    ]
                },
                new()
                {
                    Id = context.NewGuid(),
                    Name = "供应链管理中心",
                    Number = "0300",
                    Children = [
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "采购部",
                            Number = "0301",
                        },
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "物流部",
                            Number = "0302",
                        },
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "物料库",
                            Number = "0303",
                        },
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "成品库",
                            Number = "0304",
                        }
                    ]
                },
                new()
                {
                    Id = context.NewGuid(),
                    Name = "技术保障中心",
                    Number = "0400",
                    Children = [
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "设备部",
                            Number = "0401",
                        },
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "IT运维部",
                            Number = "0402",
                        },
                    ]
                },
                new()
                {
                    Id = context.NewGuid(),
                    Name = "管理保障中心",
                    Number = "0500",
                    Children = [
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "财务部",
                            Number = "0501",
                        },
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "人事部",
                            Number = "0502",
                        },
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "行政部",
                            Number = "0503",
                        },
                        new()
                        {
                            Id = context.NewGuid(),
                            Name = "后勤安保部",
                            Number = "0504",
                        }
                    ]
                }
            ]
        }.UpdateNode();
        context.Set<Department>().Add(department);
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
                new()
                {
                    Id = context.NewGuid(),
                    Name = "简体中文",
                    Number = "zh-CN"
                },
                new()
                {
                    Id = context.NewGuid(),
                    Name = "English",
                    Number = "en-US"
                }
            ]
        }.UpdateNode());
    }

    private List<Permission> InitPermission(DbContext context)
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
                    AuthType = AuthType.Permission,
                    Name = groupType.GetDisplayName(),
                    Number = groupType.Name?.TrimEnd("Attribute")!,
                    RoutePath = groupType.Name?.TrimEnd("Attribute").ToSlugify()!,
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
            var number = groupType.Name?.TrimEnd("Attribute")!;
            var group = list.FirstOrDefault(o => o.Number == number)!;
            if (groupType.BaseType != null && !groupType.BaseType.IsAbstract)
            {
                var parentNumber = groupType.BaseType!.Name!.TrimEnd("Attribute")!;
                var parent = list.FirstOrDefault(o => o.Number == parentNumber);
                group.ParentId = parent?.Id;
                group.Parent = parent;
                parent?.Children.Add(group);
            }
        });
        //添加资源菜单和资源操作按钮
        var order = 1;
        var actionDescriptors = actionProvider.ActionDescriptors.Items;
        var resourceTypeList = AppDomain.CurrentDomain.GetCustomerAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.IsAssignableTo(typeof(IResource)));
        foreach (var resourceType in resourceTypeList)
        {
            if (tenantService.TenantNumber != null && resourceType == typeof(Tenant))
            {
                continue;
            }
            // 菜单
            var resourceServiceType = typeof(IResourceService<>).MakeGenericType(resourceType);
            var controllerType = actionDescriptors.Cast<ControllerActionDescriptor>()
            .FirstOrDefault(o => o.ControllerTypeInfo.AsType().IsAssignableTo(resourceServiceType))?
            .ControllerTypeInfo.AsType();
            if (controllerType == null)
            {
                continue;
            }
            var component = resourceType.GetCustomAttribute<ViewAttribute>()?.Component ?? controllerType.GetCustomAttribute<ViewAttribute>()?.Component;
            var resourcePermission = new Permission
            {
                Id = context.NewGuid(),
                Type = MenuType.Menu,
                AuthType = AuthType.Permission,
                Name = resourceType.GetDisplayName(),
                Number = resourceType.Name.TrimEnd("Model"),
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
                    AuthType = GetAuthType(descriptor),
                    Roles = descriptor.MethodInfo.GetCustomAttribute<AuthorizeAttribute>()?.Roles,
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
                var number = group.GetType().Name?.TrimEnd("Attribute")!;
                var groupPermission = list.FirstOrDefault(o => o.Number == number);
                if (groupPermission != null)
                {
                    resourcePermission.ParentId = groupPermission.Id;
                    resourcePermission.Parent = groupPermission;
                    groupPermission.Children.Add(resourcePermission);
                }
            }
            list.Add(resourcePermission);
        }

        //设置分组首页
        list.Where(o => o.ParentId == null).ForEach(o =>
        {
            var page = o.Children.Where(o => o.Type == MenuType.Menu).OrderBy(o => o.Order).FirstOrDefault();
            if (page != null)
            {
                o.Redirect = $"/{o.Number.ToSlugify()}/{page.Number.ToSlugify()}";
            }
        });

        //更新路径
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
        return list;
    }

    private static AuthType GetAuthType(ControllerActionDescriptor descriptor)
    {
        if (descriptor.MethodInfo.GetAttributes<AllowAnonymousAttribute>().Any())
        {
            return AuthType.Anonymous;
        }
        var attribute = descriptor.MethodInfo.GetAttribute<AuthorizeAttribute>();
        if (attribute != null)
        {
            if (attribute.Roles != null)
            {
                return AuthType.Roles;
            }
            return AuthType.Authorize;
        }
        return AuthType.Permission;
    }

    private List<Role> InitRole(DbContext context, List<Permission> permissions)
    {
        if (tenantService.TenantNumber != null)
        {
            permissions.Where(o => !o.Disabled && !tenantService.Permissions.Contains(o.Number)).ForEach(o => o.Disabled = true);
            permissions = permissions.Where(o => tenantService.Permissions.Contains(o.Number)).ToList();
        }
        var rules = new List<Role> { new Role
        {
            Id = context.NewGuid(),
            Name = tenantService.TenantNumber != null ? "租户管理员" : "管理员",
            Number = "admin",
            RolePermissions = permissions!.Select(o => new RolePermission
            {
                PermissionId = o.Id,
                IsReadOnly = true
            }).ToList()
        },new()
        {
            Id = context.NewGuid(),
            Name = "测试",
            Number = "test",
        }};
        context.Set<Role>().AddRange(rules);
        return rules;
    }

    private static void InitPost(DbContext context)
    {
        var departmentId = context.Set<Department>().FirstOrDefault(o => o.Name == "研发生产中心")?.Id;
        context.Set<Post>().AddRange([new Post()
        {
            Id = context.NewGuid(),
            Name = "车间主任",
            Number = "01",
            DepartmentId = departmentId,
        }.UpdateNode(),
            new Post()
            {
                Id = context.NewGuid(),
                Name = "生产管理员",
                Number = "02",
                DepartmentId = departmentId,
            }.UpdateNode(),
            new Post()
            {
                Id = context.NewGuid(),
                Name = "物料管理员",
                Number = "03",
                DepartmentId = departmentId,
            }.UpdateNode(),
            new Post()
            {
                Id = context.NewGuid(),
                Name = "质量管理员",
                Number = "04",
                DepartmentId = departmentId,
            }.UpdateNode(),
            new Post()
            {
                Id = context.NewGuid(),
                Name = "机管员",
                Number = "05",
                DepartmentId = departmentId,
            }.UpdateNode(),
            new Post()
            {
                Id = context.NewGuid(),
                Name = "技术调控员",
                Number = "06",
                DepartmentId = departmentId,
            }.UpdateNode(),
            new Post()
            {
                Id = context.NewGuid(),
                Name = "过程调控员",
                Number = "07",
                DepartmentId = departmentId,
            }.UpdateNode(),
            new Post()
            {
                Id = context.NewGuid(),
                Name = "作业组长",
                Number = "08",
                DepartmentId = departmentId,
            }.UpdateNode(),
            new Post()
            {
                Id = context.NewGuid(),
                Name = "作业工",
                Number = "09",
                DepartmentId = departmentId,
                Children = [
                new Post()
                {
                    Id = context.NewGuid(),
                    Name = "加工组作业工",
                    Number = "0901",
                    DepartmentId = departmentId,
                }.UpdateNode(),
                    new Post()
                    {
                        Id = context.NewGuid(),
                        Name = "装配组作业工",
                        Number = "0902",
                        DepartmentId = departmentId,
                    }.UpdateNode(),
                    new Post()
                    {
                        Id = context.NewGuid(),
                        Name = "包装组作业工",
                        Number = "0903",
                        DepartmentId = departmentId,
                    }.UpdateNode(),
                    new Post()
                    {
                        Id = context.NewGuid(),
                        Name = "模具组作业工",
                        Number = "0904",
                        DepartmentId = departmentId,
                    }.UpdateNode()
            ]
            }.UpdateNode()]);
    }

    private void InitUser(DbContext context, List<Role> roles)
    {
        var userName = "admin";
        var password = "123456";
        var salt = encryptionService.CreateSalt();
        var passwordHash = encryptionService.HashPassword(password, salt);
        var email = "76527413@qq.com";

        context.Set<User>().Add(new User
        {
            Id = context.NewGuid(),
            UserName = userName,
            Email = email,
            NormalizedEmail = email.ToUpperInvariant(),
            EmailConfirmed = true,
            Name = tenantService.TenantNumber != null ? "租户管理员" : "管理员",
            NormalizedUserName = userName.ToUpperInvariant(),
            SecurityStamp = salt,
            PasswordHash = passwordHash,
            IsReadOnly = true,
            UserRoles = [
                new()
                {
                    RoleId = roles.First(o => o.Number == "admin").Id,
                    IsReadOnly = true
                }
            ],
        });
    }
}
