using System.Dynamic;

namespace Wta.Infrastructure.Extensions;

public static class DictionaryExtensions
{
    public static object DictionaryToObject(this Dictionary<string, string> dict)
    {
        var eo = new ExpandoObject();
        var eoDict = (ICollection<KeyValuePair<string, object>>)eo!;

        foreach (var item in dict)
        {
            eoDict.Add(new KeyValuePair<string, object>(item.Key, item.Value));
        }

        dynamic eoDynamic = eo;
        return eoDynamic;
    }
}
