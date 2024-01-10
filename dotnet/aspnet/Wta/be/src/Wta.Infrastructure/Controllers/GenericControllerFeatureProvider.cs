using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Wta.Infrastructure.Domain;
using Wta.Shared;

namespace Wta.Infrastructure.Controllers;

public class GenericControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        var typeInfos = WebApp.Instance.Assemblies!
            .SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.IsAssignableTo(typeof(Entity)))
            .Select(o => o.GetTypeInfo())
            .ToList();
        foreach (var entityTypeInfo in typeInfos)
        {
            var entityType = entityTypeInfo.AsType();
            if (!feature.Controllers.Any(o => o.Name == $"{entityType.Name}Controller"))
            {
                var modelType = WebApp.Instance.EntityModelDictionary.GetValueOrDefault(entityType) ?? entityType;
                var controllerType = typeof(GenericController<,>).MakeGenericType(entityType, modelType);
                feature.Controllers.Add(controllerType.GetTypeInfo());
            }
        }
    }
}
