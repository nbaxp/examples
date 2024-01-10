using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Localization;

namespace Wta.Infrastructure.Web;

public class CustomModelMetadata : DefaultModelMetadata
{
    private readonly IStringLocalizer _stringLocalizer;

    public CustomModelMetadata(IStringLocalizer stringLocalizer, IModelMetadataProvider provider, ICompositeMetadataDetailsProvider detailsProvider, DefaultMetadataDetails details, DefaultModelBindingMessageProvider modelBindingMessageProvider) : base(provider, detailsProvider, details, modelBindingMessageProvider)
    {
        this._stringLocalizer = stringLocalizer;
    }

    public override string? DisplayName
    {
        get
        {
            var name = base.DisplayName;
            if (string.IsNullOrEmpty(name))
            {
                name = this.ContainerType == null ? this.ModelType?.Name : this.PropertyName;
            }
            return _stringLocalizer.GetString(name!);
        }
    }
}
