using Mapster;

namespace Wta.Infrastructure.Mapper;

[Service<IObjerctMapper>(ServiceLifetime.Singleton)]
public class MapsterAdapter : IObjerctMapper
{
    public TEntity FromModel<TEntity, TModel>(TEntity? entity, TModel model, Action<TEntity, TModel, bool>? action = null)
    {
        var setter = TypeAdapterConfig<TModel, TEntity>.NewConfig().PreserveReference(true);
        var isNew = entity == null;
        if (isNew)
        {
            entity = Activator.CreateInstance<TEntity>();
        }
        else
        {
            setter.IgnoreAttribute(typeof(ReadOnlyAttribute));
        }
        var config = setter.Config;
        model.Adapt(entity, config);
        action?.Invoke(entity!, model, isNew);
        return entity!;
    }

    public TModel ToModel<TEntity, TModel>(TEntity entity, Action<TEntity, TModel>? action = null)
    {
        var setter = TypeAdapterConfig<TModel, TEntity>.NewConfig().PreserveReference(true);
        setter.IgnoreAttribute(typeof(IgnoreToModelAttribute));
        var model = entity.Adapt<TModel>(setter.Config);
        action?.Invoke(entity, model);
        return model;
    }
}
