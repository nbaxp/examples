using Wta.Infrastructure.Attributes;

namespace Wta.Application.Default;

public class SystemManagementAttribute : GroupAttribute
{
    public SystemManagementAttribute() : base("SystemManagement", 3, "setting")
    {
    }
}
