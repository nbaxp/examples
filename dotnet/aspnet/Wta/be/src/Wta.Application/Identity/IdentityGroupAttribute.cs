using Wta.Infrastructure.Attributes;

namespace Wta.Application.Identity;

public class IdentityGroupAttribute : GroupAttribute
{
    public IdentityGroupAttribute() : base("SystemManagement")
    {
    }
}
