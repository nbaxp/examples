using System.ComponentModel;
using Mapster;

namespace Wta.Infrastructure.Application;

public class DefaultMapper<TEntity, TModel> : IMapper<TEntity, TModel>
{
    public DefaultMapper()
    {
        var toModelConfig = TypeAdapterConfig<TEntity, TModel>.ForType();
        var toEntityConfig = TypeAdapterConfig<TModel, TEntity>.ForType();
        toEntityConfig.IgnoreMember((member, side) => side == MemberSide.Destination && member.Name == "Id");
        toEntityConfig.IgnoreMember((member, side) => side == MemberSide.Destination && member.HasCustomAttribute<ReadOnlyAttribute>());
        this.Config(toModelConfig, toEntityConfig);
    }

    protected virtual void Config(TypeAdapterSetter<TEntity, TModel> toModel, TypeAdapterSetter<TModel, TEntity> toEntity)
    {
    }

    public virtual TModel ToModel(TEntity entity)
    {
        return entity.Adapt<TModel>();
    }

    public virtual List<TModel> ToModelList(IQueryable<TEntity> entities)
    {
        return entities.ProjectToType<TModel>().ToList();
    }

    public virtual TEntity FromModel(TEntity entity, TModel model)
    {
        return model.Adapt(entity);
    }

    public virtual TDestination ToObject<TSource, TDestination>(TSource source, Action<TypeAdapterSetter<TSource, TDestination>>? config = null)
    {
        config?.Invoke(TypeAdapterConfig<TSource, TDestination>.NewConfig());
        return source.Adapt<TDestination>();
    }

    public virtual TDestination FromObject<TSource, TDestination>(TDestination source, TSource destination, Action<TypeAdapterSetter<TSource, TDestination>>? config = null)
    {
        config?.Invoke(TypeAdapterConfig<TSource, TDestination>.NewConfig());
        return destination.Adapt(source);
    }
}
