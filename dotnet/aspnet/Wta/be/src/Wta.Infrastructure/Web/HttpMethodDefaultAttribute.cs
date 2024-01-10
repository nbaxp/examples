using Microsoft.AspNetCore.Mvc.Routing;

namespace Wta.Infrastructure.Web;

public class HttpMethodDefaultAttribute : HttpMethodAttribute
{
    public HttpMethodDefaultAttribute(IEnumerable<string> httpMethods) : base(httpMethods)
    {
    }
}
