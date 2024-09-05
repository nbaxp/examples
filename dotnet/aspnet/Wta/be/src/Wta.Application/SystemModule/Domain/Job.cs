using Wta.Application.SystemModule;

namespace Wta.Infrastructure.Scheduling;

[SystemManagement, Display(Name = "定时任务", Order = 1000)]
public class Job : Entity
{
    public string Name { get; set; } = default!;
    public string Cron { get; set; } = default!;
    public string Type { get; set; } = default!;
    public bool Disabled { get; set; }
}
