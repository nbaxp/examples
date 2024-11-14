using Wta.Infrastructure.Application.Models;

namespace Wta.Application.Oee.Models;

public class OeeDashboardResult
{
    public ChartModel? Chart1 { get; set; }
    public ChartModel? Chart2 { get; set; }
    public ChartModel? Chart3 { get; set; }
}
