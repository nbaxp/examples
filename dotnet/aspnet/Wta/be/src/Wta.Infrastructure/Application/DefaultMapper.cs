using Mapster;

namespace Wta.Infrastructure.Application;

public class DefaultMapper<TEntity, TModel> : IMapper<TEntity, TModel>
{
    static DefaultMapper()
    {
        TypeAdapterConfig.GlobalSettings.Default.IgnoreMember((member, side) =>
        {
            return side == MemberSide.Destination && member.Name == "Id" && member.Type == typeof(Guid);
        });
    }

    public DefaultMapper()
    {
        this.Config(TypeAdapterConfig<TEntity, TModel>.ForType(), TypeAdapterConfig<TModel, TEntity>.ForType());
    }

    protected virtual void Config(TypeAdapterSetter<TEntity, TModel> toModel, TypeAdapterSetter<TModel, TEntity> toEntity)
    {
    }

    public TModel ToModel(TEntity entity)
    {
        return entity.Adapt<TModel>();
    }

    public List<TModel> ToModelList(IQueryable<TEntity> entities)
    {
        return entities.ProjectToType<TModel>().ToList();
    }

    public TEntity FromModel(TEntity entity, TModel model)
    {
        return model.Adapt(entity);
    }
}
