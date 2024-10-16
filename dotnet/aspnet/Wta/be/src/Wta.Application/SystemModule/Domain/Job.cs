using Wta.Application;
using Wta.Application.SystemModule.Data;

namespace Wta.Infrastructure.Scheduling;

[SystemSettings, Display(Name = "定时任务", Order = 1000)]
[DependsOn<SystemDbContext>]
public class Job : Entity
{
    public string Name { get; set; } = default!;
    public string Cron { get; set; } = default!;
    public string Type { get; set; } = default!;
    public bool Disabled { get; set; }
}
