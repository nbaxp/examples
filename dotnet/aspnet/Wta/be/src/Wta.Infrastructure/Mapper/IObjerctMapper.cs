namespace Wta.Infrastructure.Mapper;

public interface IObjerctMapper
{
    /// <summary>
    /// Create Model
    /// </summary>
    TModel ToModel<TEntity, TModel>(TEntity entity, Action<TEntity, TModel>? action = null);

    /// <summary>
    /// Create Entity
    /// </summary>
    TEntity ToEntity<TEntity, TModel>(TModel model, Action<TEntity, TModel>? action = null);

    /// <summary>
    /// Update Entity
    /// </summary>
    TEntity FromModel<TEntity, TModel>(TEntity entity, TModel model, Action<TEntity, TModel>? action = null);
}
