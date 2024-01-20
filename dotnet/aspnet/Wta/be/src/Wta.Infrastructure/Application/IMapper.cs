using Mapster;

namespace Wta.Infrastructure.Application;

public interface IMapper<TEntity, TModel>
{
    TModel ToModel(TEntity entity);

    List<TModel> ToModelList(IQueryable<TEntity> entities);

    TEntity FromModel(TEntity entity, TModel model);

    TDestination ToObject<TSource, TDestination>(TSource source, Action<TypeAdapterSetter<TSource, TDestination>>? config = null);

    TDestination FromObject<TSource, TDestination>(TDestination source, TSource destination, Action<TypeAdapterSetter<TSource, TDestination>>? config = null);
}
