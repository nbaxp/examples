using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using OrchardCore.Localization;

namespace Wta.Infrastructure.Web;

public class CustomLocalizationFileLocationProvider : ILocalizationFileLocationProvider
{
    private readonly IFileProvider _fileProvider;
    private readonly string _subpath;

    public CustomLocalizationFileLocationProvider(IFileProvider fileProvider, IOptions<LocalizationOptions> localizationOptions)
    {
        _fileProvider = fileProvider;
        _subpath = localizationOptions.Value.ResourcesPath;
    }

    public IEnumerable<IFileInfo> GetLocations(string cultureName)
    {
        var suffix = $"{cultureName}.po";
        var fileInfo = _fileProvider.GetFileInfo(Path.Combine(_subpath, suffix));
        if (fileInfo.Exists)
        {
            yield return fileInfo;
        }
    }
}
