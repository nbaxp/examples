using Wta.Application.Default.Domain;

namespace Wta.Application;

[AttributeUsage(AttributeTargets.Method)]
public class ButtonAttribute : Attribute
{
    public ButtonType Type { get; set; }
}
