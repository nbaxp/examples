using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Wta.Infrastructure.Web;

public class CustomModelMetaDataProvider : DefaultModelMetadataProvider
{
    private readonly IStringLocalizer _stringLocalizer;

    public CustomModelMetaDataProvider(IStringLocalizer stringLocalizer, ICompositeMetadataDetailsProvider detailsProvider, IOptions<MvcOptions> optionsAccessor) : base(detailsProvider, optionsAccessor)
    {
        _stringLocalizer = stringLocalizer;
    }

    protected override ModelMetadata CreateModelMetadata(DefaultMetadataDetails entry)
    {
        return new CustomModelMetadata(_stringLocalizer, this, DetailsProvider, entry, ModelBindingMessageProvider);
    }
}
