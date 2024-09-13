using Flurl;

namespace Wta.Infrastructure.Extensions;

public static class UrlExtensions
{
    public static Url SetQueryParamIf(this Url url, bool expression, string name, string? value)
    {
        return expression ? url.SetQueryParam(name, value) : url;
    }
}
