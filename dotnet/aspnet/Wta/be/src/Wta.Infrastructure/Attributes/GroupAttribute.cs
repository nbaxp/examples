namespace Wta.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class GroupAttribute : Attribute
{
    public GroupAttribute(string name, int order = 0, string? icon = null)
    {
        this.Name = name;
        this.Order = order;
        this.Icon = icon ?? "folder";
    }

    public string Name { get; set; }
    public int Order { get; set; }
    public string Icon { get; set; }
}
