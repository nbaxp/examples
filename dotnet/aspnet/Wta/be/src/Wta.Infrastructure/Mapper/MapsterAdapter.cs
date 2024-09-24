using System.Collections.Concurrent;
using Mapster;

namespace Wta.Infrastructure.Mapper;

//[Service<IObjerctMapper>(ServiceLifetime.Singleton)]
public class MapsterAdapter : IObjerctMapper
{
    public static ConcurrentDictionary<string, object> Maps = new ConcurrentDictionary<string, object>();

    public TEntity FromModel<TEntity, TModel>(TEntity entity, TModel model, Action<TEntity, TModel, bool>? action = null, bool isCreate = false)
    {
        var setter = GetConfig<TModel, TEntity>();
        var isNew = entity == null;
        if (!isNew)
        {
            setter.IgnoreAttribute(typeof(ReadOnlyAttribute));
        }
        var config = setter.Config;
        model.Adapt(entity, config);
        action?.Invoke(entity!, model, isCreate);
        return entity!;
    }

    public TModel ToModel<TEntity, TModel>(TEntity entity, Action<TEntity, TModel>? action = null)
    {
        var setter = GetConfig<TEntity, TModel>();
        setter.IgnoreAttribute(typeof(IgnoreToModelAttribute));
        var config = setter.Config;
        var model = Activator.CreateInstance<TModel>();
        model = entity.Adapt(model, setter.Config);
        action?.Invoke(entity, model);
        return model;
    }

    public static TypeAdapterSetter<TSource, TTarget> GetConfig<TSource, TTarget>(int depth = 1)
    {
        return TypeAdapterConfig<TSource, TTarget>.NewConfig().PreserveReference(true).MaxDepth(depth);
    }
}
