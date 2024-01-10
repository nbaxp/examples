namespace Wta.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class OrderAttribute : Attribute
{
    public OrderAttribute(int order = 0) => Order = order;

    public int Order { get; set; }
}
