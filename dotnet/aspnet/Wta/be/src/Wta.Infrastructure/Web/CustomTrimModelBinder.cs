using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wta.Infrastructure.Web;

public class CustomTrimModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.Result = ModelBindingResult.Success(valueProviderResult.FirstValue?.Trim());
        return Task.CompletedTask;
    }
}

public class CustomModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.Metadata.ModelType == typeof(string) && context.BindingInfo.BindingSource != BindingSource.Body)
        {
            return new CustomTrimModelBinder();
        }

        return null;
    }
}
