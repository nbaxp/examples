using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace Wta.Infrastructure.Web;

/// <summary>
/// 修复.net嵌入式资源路径问题
/// </summary>
public class CustomFileProvider : EmbeddedFileProvider, IFileProvider
{
    public CustomFileProvider(Assembly assembly, string? baseNamespace) : base(assembly, baseNamespace)
    {
    }

    public new IFileInfo GetFileInfo(string subpath)
    {
        if (subpath.LastIndexOf('-') > 0 || subpath.LastIndexOf('@') > 0)
        {
            var path = subpath[..(subpath.LastIndexOf('/') + 1)];
            var fileName = Path.GetFileName(subpath);
            return base.GetFileInfo($"{path.Replace('-', '_').Replace('@', '_')}{fileName}");
        }
        return base.GetFileInfo(subpath);
    }
}