namespace Wta.Infrastructure.Application;

public interface IMapper<TEntity, TModel>
{
    TModel ToModel(TEntity entity);

    List<TModel> ToModelList(IQueryable<TEntity> entities);

    TEntity FromModel(TEntity entity, TModel model);
}
