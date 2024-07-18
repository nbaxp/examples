namespace Wta.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public abstract class GroupAttribute : Attribute
{
    public GroupAttribute(string? redirect = null)
    {
        this.Redirect = redirect;
    }

    public string? Redirect { get; }
}
