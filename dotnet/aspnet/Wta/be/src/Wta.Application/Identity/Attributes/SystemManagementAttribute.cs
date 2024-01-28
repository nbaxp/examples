using Wta.Infrastructure.Attributes;

namespace Wta.Application.Identity.Attributes;

public class SystemManagementAttribute : GroupAttribute
{
    public SystemManagementAttribute() : base("SystemManagement", 3, "setting")
    {
    }
}
