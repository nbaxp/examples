using Wta.Infrastructure.Services;

namespace Wta.Infrastructure.Interfaces;

public interface IMapperConfig<TSource, TDestination>
{
    void Config(MapperConfigBuilder<TSource, TDestination> builder);
}
