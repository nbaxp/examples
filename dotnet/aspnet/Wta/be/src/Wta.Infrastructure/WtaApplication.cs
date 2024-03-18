using Microsoft.AspNetCore.Builder;
using Wta.Infrastructure.Application.Models;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Extensions;
using Wta.Infrastructure.Startup;

namespace Wta.Infrastructure;

public static class WtaApplication
{
    public static WebApplication Application { get; private set; } = default!;

    public static List<Assembly> Assemblies { get; private set; } = [];
    public static WebApplicationBuilder Builder { get; private set; } = default!;
    public static Dictionary<Type, List<Type>> DbContextEntities { get; } = [];
    public static Dictionary<Type, Type> EntityDbContext { get; } = [];
    public static Dictionary<Type, Type> EntityModel { get; } = [];
    public static Dictionary<Type, List<Type>> ModuleDbContexts { get; } = [];

    public static void Initialize()
    {
        Assemblies.Add(Assembly.GetEntryAssembly()!);
        Assemblies.Add(Assembly.GetExecutingAssembly());
        //加载实体和数据上下文关系
        ////获取配置类
        Assemblies.SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
            .ForEach(item =>
            {
                var dbContextType = item.GetCustomAttribute(typeof(DependsOnAttribute<>))!.GetType().GenericTypeArguments.First();
                var moduleType = dbContextType.GetCustomAttribute(typeof(DependsOnAttribute<>))!.GetType().GenericTypeArguments.First();
                if (moduleType.IsAssignableTo(typeof(IStartup)))
                {
                    if (ModuleDbContexts.TryGetValue(moduleType, out var dbContextTypeList))
                    {
                        dbContextTypeList.Add(dbContextType);
                    }
                    else
                    {
                        ModuleDbContexts.Add(moduleType, [dbContextType]);
                    }
                }
                var entityTypes = item.GetInterfaces().Where(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                .Select(o => o.GetGenericArguments()[0]);
                entityTypes.ForEach(item =>
                {
                    EntityDbContext.TryAdd(item, dbContextType);
                });
                if (DbContextEntities.TryGetValue(dbContextType, out var entityTypeList))
                {
                    entityTypeList.AddRange(entityTypes);
                }
                else
                {
                    DbContextEntities.Add(dbContextType, entityTypes.ToList());
                }
            });
        // 缓存实体和模型关系
        Assemblies.SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IBaseModel<>)))
            .ForEach(item =>
            {
                var entityType = item.GetInterfaces()
                .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IBaseModel<>))!
                .GenericTypeArguments[0];
                EntityModel.Add(entityType, item);
            });
    }

    public static void Run<T>(string[] args)
        where T : IStartup
    {
        typeof(T).GetCustomAttributes(typeof(DependsOnAttribute<>))
            .Select(o => o.GetType().GenericTypeArguments.First().Assembly)
            .Distinct()
            .ForEach(Assemblies.Add);
        Initialize();
        var startup = Activator.CreateInstance<T>()!;
        var modules = ModuleDbContexts.Keys.Select(o => Activator.CreateInstance(o) as IStartup);
        Console.WriteLine("Configuring web host...");
        Builder = WebApplication.CreateBuilder(args);
        startup.ConfigureServices(Builder);
        modules.ForEach(o => o!.ConfigureServices(Builder));
        Application = Builder.Build();
        startup.Configure(Application);
        modules.ForEach(o => o!.Configure(Application));
        Console.WriteLine("Starting web host...");
        Application.Run();
        try
        {
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            Console.WriteLine("Program terminated unexpectedly!");
        }
        finally
        {
            Console.WriteLine("Program exted!");
        }
    }
}
