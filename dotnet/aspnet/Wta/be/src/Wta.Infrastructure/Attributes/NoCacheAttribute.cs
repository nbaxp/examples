namespace Wta.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class NoCacheAttribute(bool noCache = true) : Attribute
{
    public bool NoCache { get; set; } = noCache;
}
