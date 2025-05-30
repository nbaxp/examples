using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Wta.Infrastructure.Mapper;

[Service<IObjerctMapper>(ServiceLifetime.Singleton)]
public class ValueInjecterAdapter : IObjerctMapper
{
    public TEntity FromModel<TEntity, TModel>(TEntity entity, TModel model)
    {
        List<string> ignoreNames = ["Id"];
        ignoreNames.AddRange(typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(o => o.HasAttribute<ReadOnlyAttribute>()).Select(o => o.Name));
        entity.InjectFrom(new NullableInjection([.. ignoreNames]), model);
        return entity!;
    }

    public TEntity ToEntity<TEntity, TModel>(TModel model)
    {
        List<string> ignoreNames = ["Id"];
        var entity = Activator.CreateInstance<TEntity>();
        entity.InjectFrom(new NullableInjection([.. ignoreNames]), model);
        return entity;
    }

    public TModel ToModel<TEntity, TModel>(TEntity entity)
    {
        var ignoreNames = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(o => o.HasAttribute<IgnoreToModelAttribute>())
            .Select(o => o.Name)
            .ToArray();
        var model = Activator.CreateInstance<TModel>();
        model.InjectFrom(new NullableInjection(ignoreNames), entity);
        return model;
    }
}

public class NullableInjection : LoopInjection
{
    public NullableInjection(string[] ignoredProps) : base(ignoredProps)
    {
    }

    protected override bool MatchTypes(Type sourceType, Type targetType)
    {
        return sourceType == targetType
               || sourceType == targetType.GetUnderlyingType()
               || targetType == sourceType.GetUnderlyingType();
    }
}
