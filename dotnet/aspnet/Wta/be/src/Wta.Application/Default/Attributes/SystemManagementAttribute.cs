using Wta.Infrastructure.Attributes;

namespace Wta.Application.Default.Attributes;

public class SystemManagementAttribute : GroupAttribute
{
    public SystemManagementAttribute() : base("SystemManagement", 3, "setting")
    {
    }
}
