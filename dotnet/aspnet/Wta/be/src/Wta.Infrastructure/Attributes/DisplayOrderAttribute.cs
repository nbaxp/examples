namespace Wta.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class DisplayOrderAttribute(int order = 0) : Attribute
{
    public int Order { get; set; } = order;
}
