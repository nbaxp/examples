namespace Wta.Infrastructure.Extensions;

public static class MapperExtensions
{
    //public static TModel ToModel<TEntity, TModel>(this TEntity entity, Action<TEntity, TModel>? action = null)
    //{
    //    var mapper = WtaApplication.Application.Services.GetRequiredService<IObjerctMapper>();
    //    var model = mapper.ToModel<TEntity, TModel>(entity);
    //    action?.Invoke(entity, model);
    //    return model;
    //}

    //public static TEntity FromModel<TEntity, TModel>(this TEntity entity, TModel model, Action<TEntity, TModel, bool>? action = null, bool isCreate = false)
    //{
    //    var mapper = WtaApplication.Application.Services.GetRequiredService<IObjerctMapper>();
    //    mapper.FromModel(entity, model);
    //    action?.Invoke(entity, model, isCreate);
    //    return entity;
    //}
}
