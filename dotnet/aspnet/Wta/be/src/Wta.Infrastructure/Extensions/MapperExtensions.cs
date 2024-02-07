using Mapster;
using Wta.Infrastructure.Interfaces;
using Wta.Infrastructure.Services;
using Wta.Shared;

namespace Wta.Infrastructure.Extensions;

public static class MapperExtensions
{
    public static TModel ToModel<TEntity, TModel>(this TEntity entity, Action<TypeAdapterSetter<TEntity, TModel>>? action = null)
    {
        return entity.Adapt<TModel>(GetConfig(action));
    }

    public static TModel? ToModel<TEntity, TModel>(this IQueryable<TEntity> query, Action<TypeAdapterSetter<TEntity, TModel>>? action = null)
    {
        return query.ProjectToType<TModel>(GetConfig(action)).FirstOrDefault();
    }

    public static List<TModel> ToModelList<TEntity, TModel>(this IQueryable<TEntity> query, Action<TypeAdapterSetter<TEntity, TModel>>? action = null)
    {
        return query.ProjectToType<TModel>(GetConfig(action)).ToList();
    }

    public static TEntity FromModel<TEntity, TModel>(this TEntity entity, TModel model, Action<TypeAdapterSetter<TModel, TEntity>>? action = null)
    {
        model.Adapt(entity, GetConfig(action));
        return entity;
    }

    private static TypeAdapterConfig GetConfig<TSource, TDestination>(Action<TypeAdapterSetter<TSource, TDestination>>? action)
    {
        var mapperConfig = WebApp.Instance.WebApplication.Services.GetService<IMapperConfig<TSource, TDestination>>();
        TypeAdapterSetter<TSource, TDestination>? typeConfig;
        if (mapperConfig != null)
        {
            var builder = new MapperConfigBuilder<TSource, TDestination>();
            mapperConfig.Config(builder);
            typeConfig = builder.Config;
        }
        else
        {
            typeConfig = TypeAdapterConfig<TSource, TDestination>.NewConfig();
        }
        action?.Invoke(typeConfig);
        return typeConfig.Config;
    }
}
