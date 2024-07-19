namespace Wta.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class KeyValueAttribute(string key, object value) : Attribute
{
    public string Key { get; set; } = key;
    public object Value { get; set; } = value;
}
