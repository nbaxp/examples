using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Reflection;

namespace WpfApp1;

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