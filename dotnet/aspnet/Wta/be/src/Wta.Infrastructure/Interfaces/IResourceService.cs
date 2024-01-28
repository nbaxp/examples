using Wta.Infrastructure.Domain;

namespace Wta.Infrastructure.Interfaces;

public interface IResourceService
{
}

public interface IResourceService<TResource> : IResourceService
    where TResource : IResource
{
}
