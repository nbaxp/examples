namespace Wta.Infrastructure.Mapper;

public interface IObjerctMapper
{
    TModel ToModel<TEntity, TModel>(TEntity entity, Action<TEntity, TModel>? action = null);

    TEntity FromModel<TEntity, TModel>(TEntity entity, TModel model, Action<TEntity, TModel, bool>? action = null, bool isCreate = false);
}
