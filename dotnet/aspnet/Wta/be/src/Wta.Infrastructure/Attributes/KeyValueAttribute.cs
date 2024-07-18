namespace Wta.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class KeyValueAttribute(string key, string value) : Attribute
{
    public string Key { get; set; } = key;
    public string Value { get; set; } = value;
}
