namespace Wta.Infrastructure;

public static class Global
{
    public static WebApplication Application { get; set; } = default!;
    //public static Dictionary<Type, Type> EntityModel { get; } = [];
    //public static Dictionary<Type, List<Type>> ModuleDbContexts { get; } = [];

    //public static void Initialize()
    //{
    //    //加载实体和数据上下文关系
    //    ////获取配置类
    //    AppDomain.CurrentDomain.GetCustomerAssemblies()
    //        .SelectMany(o => o.GetTypes())
    //        .Where(o => !o.IsAbstract && o.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
    //        .ForEach(item =>
    //        {
    //            var dbContextType = item.GetBaseClasses().First(o => o.IsGenericType && o.GetGenericTypeDefinition() == typeof(BaseDbConfig<>)).GenericTypeArguments.First();
    //            var moduleType = dbContextType.GetCustomAttribute(typeof(DependsOnAttribute<>))!.GetType().GenericTypeArguments.First();
    //            if (moduleType.IsAssignableTo(typeof(IStartup)))
    //            {
    //                if (ModuleDbContexts.TryGetValue(moduleType, out var dbContextTypeList))
    //                {
    //                    dbContextTypeList.Add(dbContextType);
    //                }
    //                else
    //                {
    //                    ModuleDbContexts.Add(moduleType, [dbContextType]);
    //                }
    //            }
    //        });
    //    // 缓存实体和模型关系
    //    AppDomain.CurrentDomain.GetCustomerAssemblies()
    //        .SelectMany(o => o.GetTypes())
    //        .Where(o => o.IsClass &&
    //        !o.IsAbstract &&
    //        o.GetCustomAttributes(typeof(DependsOnAttribute<>)).Any(o => o.GetType().GenericTypeArguments.First().IsAssignableTo(typeof(IResource))))
    //        .ForEach(item =>
    //        {
    //            var entityType = item.GetCustomAttribute(typeof(DependsOnAttribute<>))?.GetType().GenericTypeArguments.First()!;
    //            EntityModel.Add(entityType, item);
    //        });
    //}
}
