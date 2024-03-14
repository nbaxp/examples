using Microsoft.AspNetCore.Builder;
using Wta.Infrastructure.Models;
using Wta.Shared;
using Wta.Shared.Data;

namespace Wta.Infrastructure.Hosting;

public static class WtaApplication
{
    public static WebApplication Application { get; private set; } = default!;

    public static List<Assembly> Assemblies { get; private set; } = [];
    public static WebApplicationBuilder Builder { get; private set; } = default!;
    public static Dictionary<Type, List<Type>> DbContextEntityDictionary { get; } = new Dictionary<Type, List<Type>>();
    public static Dictionary<Type, Type> EntityDbContextDictionary { get; } = new Dictionary<Type, Type>();
    public static Dictionary<Type, Type> EntityModels { get; } = new Dictionary<Type, Type>();
    public static void Initialize()
    {
        Assemblies.Add(Assembly.GetEntryAssembly()!);
        Assemblies.Add(Assembly.GetExecutingAssembly());
        //加载实体和数据上下文关系
        ////获取配置类
        Assemblies.SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IDbConfig<>)))
            .ForEach(item =>
            {
                var dbContextType = item.GetInterfaces().First(o => o.IsGenericType && o.GetGenericTypeDefinition() == typeof(IDbConfig<>)).GetGenericArguments()[0];
                var entityTypes = item.GetInterfaces().Where(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                .Select(o => o.GetGenericArguments()[0]);
                entityTypes.ForEach(item =>
                {
                    EntityDbContextDictionary.TryAdd(item, dbContextType);
                });
                if (DbContextEntityDictionary.TryGetValue(dbContextType, out var list))
                {
                    list.AddRange(entityTypes);
                }
                else
                {
                    DbContextEntityDictionary.Add(dbContextType, entityTypes.ToList());
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
                EntityModels.Add(entityType, item);
            });
    }

    public static void Load<T>()
    {
        Assemblies.Add(typeof(T).Assembly);
    }
    public static void Run<T>(string[] args)
        where T : IStartup
    {
        var startup = Activator.CreateInstance<T>()!;
        try
        {
            startup.Initialize();
            Console.WriteLine("Configuring web host...");
            Builder = WebApplication.CreateBuilder(args);
            startup.ConfigureServices(Builder);
            Application = Builder.Build();
            startup.Configure(Application);
            Console.WriteLine("Starting web host...");
            Application.Run();
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
