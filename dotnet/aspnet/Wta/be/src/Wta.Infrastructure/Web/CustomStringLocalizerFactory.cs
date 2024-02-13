//using System.Globalization;
//using System.Text.Json;
//using Microsoft.Extensions.Caching.Distributed;
//using Wta.Infrastructure.Domain;

//namespace Wta.Infrastructure.Web;

//public class CustomStringLocalizerFactory(IServiceProvider serviceProvider,IDistributedCache distributedCache) : IStringLocalizerFactory
//{
//    public IStringLocalizer Create(Type resourceSource)
//    {
//        return new CustomStringLocalizer(serviceProvider, distributedCache);
//    }

//    public IStringLocalizer Create(string baseName, string location)
//    {
//        return new CustomStringLocalizer(serviceProvider, distributedCache);
//    }
//}

//public class Resource : BaseEntity
//{
//    public string Culture { get; set; } = default!;
//    public string Name { get; set; } = default!;
//    public string Value { get; set; } = default!;
//}

//public class CustomStringLocalizer<T>(IServiceProvider serviceProvider, IDistributedCache distributedCache) : IStringLocalizer<T>
//{
//    public LocalizedString this[string name]
//    {
//        get
//        {
//            var value = GetString(name);
//            return new LocalizedString(name, value ?? name, value == null);
//        }
//    }

//    public LocalizedString this[string name, params object[] arguments]
//    {
//        get
//        {
//            var actualValue = this[name];
//            return !actualValue.ResourceNotFound
//                ? new LocalizedString(name, string.Format(CultureInfo.InvariantCulture, actualValue.Value, arguments), false)
//                : actualValue;
//        }
//    }

//    public IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures)
//    {
//        return _context.Resources
//            .Include(r => r.Culture)
//            .Where(r => r.Culture.Name == CultureInfo.CurrentCulture.Name)
//            .Select(r => new LocalizedString(r.Key, r.Value, true));
//    }

//    private string GetString(string name)
//    {
//        return _context.Resources
//            .Include(r => r.Culture)
//            .Where(r => r.Culture.Name == CultureInfo.CurrentCulture.Name)
//            .FirstOrDefault(r => r.Key == name)?.Value;
//    }
//}

//public class CustomStringLocalizer(IServiceProvider serviceProvider, IDistributedCache distributedCache) : IStringLocalizer
//{
//    public LocalizedString this[string name]
//    {
//        get
//        {
//            var value = GetString(name);
//            return new LocalizedString(name, value ?? name, value == null);
//        }
//    }

//    public LocalizedString this[string name, params object[] arguments]
//    {
//        get
//        {
//            var actualValue = this[name];
//            return !actualValue.ResourceNotFound
//                ? new LocalizedString(name, string.Format(CultureInfo.InvariantCulture, actualValue.Value, arguments), false)
//                : actualValue;
//        }
//    }

//    public IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures)
//    {
//        return JsonSerializer.Deserialize<Dictionary<string,string>>(distributedCache.GetString(CultureInfo.CurrentCulture.Name))
//            .Select(o => new LocalizedString(o.Key, o.Value));
//    }

//    private string GetString(string name)
//    {
//        var key = $"{CultureInfo.CurrentCulture.Name}/{name}";
//        var value = distributedCache.GetString(key)
//        if (value==null)
//        {
//            value = serviceProvider.GetRequiredService<IRepository>
//        }
//        return distributedCache.GetString()??name;
//    }
//}
