namespace Wta.Infrastructure.Mapper;

public interface IEntityModelMapper<TEntity, TModel> where TEntity : class
{
    TEntity FromModel(TEntity entity, TModel model);

    TModel ToModel(TEntity entity);
}

public class EntityModelMapper<TEntity, TModel> : IEntityModelMapper<TEntity, TModel> where TEntity : class
{
    public TEntity FromModel(TEntity entity, TModel model)
    {
        return entity;
    }

    public virtual TModel ToModel(TEntity entity)
    {
        var model = Activator.CreateInstance<TModel>();
        return model;
    }
}

//[Service<IEntityModelMapper<Department,Department>>(ServiceLifetime.Scoped)]
//public class DepartmentMapper(IRepository<Department> repository) : EntityModelMapper<Department, Department>
//{
//    public override Department FromModel(Department model)
//    {
//        var entity = base.FromModel(model);
//        entity.
//        return entity;
//    }
//}
