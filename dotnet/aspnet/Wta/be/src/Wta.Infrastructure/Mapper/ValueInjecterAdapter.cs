using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Wta.Infrastructure.Mapper;

[Service<IObjerctMapper>(ServiceLifetime.Singleton)]
public class ValueInjecterAdapter : IObjerctMapper
{
    //static ValueInjecterAdapter()
    //{
    //    Omu.ValueInjecter.Mapper.Instance.DefaultMap = 
    //}
    public TEntity FromModel<TEntity, TModel>(TEntity? entity, TModel model, Action<TEntity, TModel, bool>? action = null)
    {
        var isNew = entity == null;
        List<string> ignoreNames = ["Id"];
        if (isNew)
        {
            entity = Activator.CreateInstance<TEntity>();
            entity.InjectFrom(new LoopInjection([.. ignoreNames]), model);
        }
        else
        {
            ignoreNames.AddRange(typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(o => o.HasAttribute<ReadOnlyAttribute>()).Select(o => o.Name));
            entity.InjectFrom(new LoopInjection([.. ignoreNames]), model);
        }
        action?.Invoke(entity!, model, isNew);
        return entity!;
    }

    public TModel ToModel<TEntity, TModel>(TEntity entity, Action<TEntity, TModel>? action = null)
    {
        var model = Omu.ValueInjecter.Mapper.Map<TModel>(entity);
        action?.Invoke(entity, model);
        return model;
    }
}
