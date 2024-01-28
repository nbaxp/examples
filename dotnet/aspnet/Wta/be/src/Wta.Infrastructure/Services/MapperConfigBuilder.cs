using System.Linq.Expressions;
using Mapster;

namespace Wta.Infrastructure.Services;

public class MapperConfigBuilder<TSource, TDestination>
{
    public readonly TypeAdapterSetter<TSource, TDestination> Config;

    public MapperConfigBuilder()
    {
        Config = TypeAdapterConfig<TSource, TDestination>.NewConfig();
    }

    public MapperConfigBuilder<TSource, TDestination> Ignore(Expression<Func<TDestination, object>> expression)
    {
        Config.Ignore(expression);
        return this;
    }

    public MapperConfigBuilder<TSource, TDestination> Ignore<TAttribute>() where TAttribute : Attribute
    {
        Config.IgnoreAttribute(typeof(TAttribute));
        return this;
    }

    public MapperConfigBuilder<TSource, TDestination> Map<TDestinationMember, TSourceMember>(Expression<Func<TDestination, TDestinationMember>> member, Expression<Func<TSource, TSourceMember>> source)
    {
        Config.Map(member, source);
        return this;
    }
}
