using Wta.Infrastructure.Domain;

namespace Wta.Infrastructure.Abstractions;

public interface IResourceService
{
}

public interface IResourceService<TResource> : IResourceService
    where TResource : IResource
{
}
