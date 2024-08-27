using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Wta.Infrastructure.Mapper;

//[Service<IObjerctMapper>(ServiceLifetime.Singleton)]
public class ValueInjecterAdapter : IObjerctMapper
{
    public TEntity FromModel<TEntity, TModel>(TEntity? entity, TModel model, Action<TEntity, TModel, bool>? action = null)
    {
        var isNew = entity == null;
        List<string> ignoreNames = ["Id"];
        if (isNew)
        {
            entity = Activator.CreateInstance<TEntity>();
            entity.InjectFrom(new NullableInjection([.. ignoreNames]), model);
        }
        else
        {
            ignoreNames.AddRange(typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(o => o.HasAttribute<ReadOnlyAttribute>())
                .Select(o => o.Name));
            entity.InjectFrom(new NullableInjection([.. ignoreNames]), model);
        }
        action?.Invoke(entity!, model, isNew);
        return entity!;
    }

    public TModel ToModel<TEntity, TModel>(TEntity entity, Action<TEntity, TModel>? action = null)
    {
        var ignoreNames = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(o => o.HasAttribute<IgnoreToModelAttribute>())
            .Select(o => o.Name)
            .ToArray();
        var model = Activator.CreateInstance<TModel>();
        entity.InjectFrom(new NullableInjection(ignoreNames), model);
        action?.Invoke(entity, model);
        return model;
    }
}

//public class NullableInjection : FlatLoopInjection
//{
//    protected string[]? ignoredProps;

//    public NullableInjection(string[]? ignoredProps = null)
//    {
//        this.ignoredProps = ignoredProps;
//    }

//    protected override bool Match(string propName, PropertyInfo unflatProp, PropertyInfo targetFlatProp)
//    {
//        if (ignoredProps != null && ignoredProps.Contains(propName))
//        {
//            return false;
//        }
//        return unflatProp.PropertyType.UnderlyingSystemType == targetFlatProp.PropertyType.UnderlyingSystemType && propName == unflatProp.Name && unflatProp.GetGetMethod() != null;
//    }
//}

public class NullableInjection : LoopInjection
{
    public NullableInjection(string[] ignoredProps) : base(ignoredProps)
    {
    }

    protected override void Execute(PropertyInfo sp, object source, object target)
    {
        if (sp.CanRead && sp.GetGetMethod() != null && (ignoredProps == null || !ignoredProps.Contains(sp.Name)))
        {
            var tp = target.GetType().GetProperty(sp.Name);
            if (tp != null && tp.CanWrite && tp.PropertyType.UnderlyingSystemType == sp.PropertyType.UnderlyingSystemType && tp.GetSetMethod() != null)
            {
                tp.SetValue(target, sp.GetValue(source, null), null);
            }
        }
    }
}
