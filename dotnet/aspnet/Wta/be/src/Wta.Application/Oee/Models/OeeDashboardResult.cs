using Wta.Infrastructure.Application.Models;

namespace Wta.Application.Oee.Models;

public class OeeDashboardResult
{
    public ChartModel? Asset { get; set; }
    public ChartModel? Components { get; set; }
    public ChartModel? Trend { get; set; }
}
