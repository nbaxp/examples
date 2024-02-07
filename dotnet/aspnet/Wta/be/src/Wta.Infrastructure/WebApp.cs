using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Wta.Infrastructure.Extensions;
using Wta.Infrastructure.Models;
using Wta.Shared.Data;

namespace Wta.Shared;

public class WebApp
{
    /// <summary>
    /// 预加载的自定义程序集
    /// </summary>
    public List<Assembly> Assemblies { get; } = new List<Assembly>();

    public WebApplicationBuilder WebApplicationBuilder { get; }

    public WebApplication WebApplication { get; private set; } = default!;

    /// <summary>
    /// WebApp实例
    /// </summary>
    public static WebApp Instance { get; private set; } = default!;

    /// <summary>
    /// 数据上下文和实体映射字典
    /// </summary>
    public Dictionary<Type, List<Type>> DbContextEntityDictionary { get; } = new Dictionary<Type, List<Type>>();

    /// <summary>
    /// 实体和数据上下文映射字典
    /// </summary>
    public Dictionary<Type, Type> EntityDbContextDictionary { get; } = new Dictionary<Type, Type>();

    /// <summary>
    /// 实体和模型映射字典
    /// </summary>
    public Dictionary<Type, Type> EntityModelDictionary { get; } = new Dictionary<Type, Type>();

    /// <summary>
    /// 配置并启动应用
    /// </summary>
    /// <param name="args"></param>
    /// <param name="webApplicationBilderAction"></param>
    /// <param name="webApplicationAction"></param>
    public static void Start(string[] args, Action<WebApplicationBuilder>? webApplicationBilderAction = null, Action<WebApplication>? webApplicationAction = null)
    {
        Instance = new WebApp(args);
        Instance.ConfigureServices(webApplicationBilderAction);
        Instance.Configure(webApplicationAction);
        Instance.Run();
    }

    public WebApp(string[] args)
    {
        //初始化配置

        var configurationBuilder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .AddCommandLine(args);

        //初始化日志
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Async(o => o.Console(formatProvider: CultureInfo.InvariantCulture))
            .WriteTo.Async(o => o.File(Path.Combine(AppContext.BaseDirectory, $"../logs/{Assembly.GetEntryAssembly()!.GetName().Name}_.txt"),
            rollingInterval: RollingInterval.Day,
            fileSizeLimitBytes: 1024 * 1024 * 10,
            formatProvider: CultureInfo.InvariantCulture))
            .ReadFrom.Configuration(configurationBuilder.Build())
            .CreateLogger();

        //加载的自定义程序集
        ////https://dotnetcoretutorials.com/getting-assemblies-is-harder-than-you-think-in-c/
        var assemblyFullName = GetType().Assembly.FullName!;
        var prefix = assemblyFullName[..assemblyFullName.IndexOf(".")];
        Directory.GetFiles(Path.GetDirectoryName(AppContext.BaseDirectory)!, $"{prefix}.*.dll").ForEach(file =>
        {
            Assemblies.Add(Assembly.LoadFrom(file));
            Log.Information(file);
        });

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
                EntityModelDictionary.Add(entityType, item);
            });

        //创建WebApplicationBuilder
        WebApplicationBuilder = WebApplication.CreateBuilder(args);

        //配置日志
        WebApplicationBuilder.Host.UseSerilog();
    }

    /// <summary>
    /// 配置依赖注入
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public WebApp ConfigureServices(Action<WebApplicationBuilder>? action = null)
    {
        //默认依赖注入
        WebApplicationBuilder.ConfigureServices();
        //自定义依赖注入
        action?.Invoke(WebApplicationBuilder);
        return this;
    }

    /// <summary>
    /// 配置应用
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public WebApp Configure(Action<WebApplication>? action = null)
    {
        //创建WebApplication
        WebApplication = WebApplicationBuilder.Build();
        //默认应用配置
        WebApplication.Configure();
        //自定义应用配置
        action?.Invoke(WebApplication);
        return this;
    }

    public void Run()
    {
        try
        {
            Log.Information("Application start");
            //启动应用
            WebApplication.Run();
        }
        catch (Exception ex)
        {
            //应用级别异常
            Log.Fatal(ex, "Host terminated");
        }
        finally
        {
            //退出应用
            Log.Information("Application exit");
            Log.CloseAndFlush();
        }
    }
}
