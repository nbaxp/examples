using Wta.Application.Oee.Domain;
using Wta.Application.Oee.Models;
using Wta.Application.Oee.Resources;

namespace Wta.Application.Platform.Controllers;

public class OeeDashboardController(IRepository<OeeAsset> oeeAssetRepository,
    IRepository<OeeStatus> oeeStatusRepository,
    IRepository<OeeData> oeeDataRepository) : BaseController, IResourceService<OeeDashboard>
{
    [HttpGet, AllowAnonymous, Ignore]
    public ApiResult<object> Schema()
    {
        return Json(typeof(OeeDashboard).GetMetadataForType());
    }

    [Display(Name = "OEE仪表盘")]
    [Authorize]
    [HttpPost]
    public ApiResult<OeeDashboardResult> Index(OeeDashboard model)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException();
        }
        var assetPath = oeeAssetRepository.AsNoTracking().Where(o => o.Number == model.AssetNumber).Select(o => o.Path).FirstOrDefault() ?? "/";
        var assetList = oeeAssetRepository.AsNoTracking().Where(o => o.Path.StartsWith(assetPath)).ToList();
        var assetNumbers = assetList.Select(o => o.Number).ToList();
        var assetStatusList = oeeStatusRepository.AsNoTracking().ToList();
        var planned = assetStatusList.Where(o => o.Type == OeeStatusType.PlannedDowntime).Select(o => o.Number).ToList();
        var production = assetStatusList.Where(o => o.Type == OeeStatusType.ProductionTime).Select(o => o.Number).ToList();
        var running = assetStatusList.Where(o => o.Type == OeeStatusType.ProductionTime || o.Type == OeeStatusType.UnloadedTime).Select(o => o.Number).ToList();

        var query = oeeDataRepository.AsNoTracking()
            .Where(o => assetNumbers.Contains(o.AssetNumber))
            .Where(o => o.Date >= model.Start && o.Date <= model.End);

        var data = query
            .GroupBy(o => new
            {
                o.AssetNumber,
                o.Date,
                o.StatusNumber,
                o.Duration,
                o.TotalItems,
                o.ScrapItems,
            })
            .Select(g => new
            {
                g.Key.AssetNumber,
                g.Key.Date,
                g.Key.StatusNumber,
                Duration = g.Sum(o => o.Duration),
                TotalItems = g.Sum(o => o.TotalItems),
                ScrapItems = g.Sum(o => o.ScrapItems),
            })
            .ToList();

        var assetData = data
            .GroupBy(o => o.AssetNumber)
            .Select(g => new
            {
                g.Key,
                Availability = 1f * g.Where(o => production.Contains(o.StatusNumber)).Sum(o => o.Duration) / g.Where(o => !planned.Any(ps => ps == o.StatusNumber)).Sum(o => o.Duration),
                Performance = 1f * g.Where(o => production.Contains(o.StatusNumber)).Sum(o => o.Duration) / g.Where(o => running.Contains(o.StatusNumber)).Sum(o => o.Duration),
                Quality = 1f * g.Sum(o => o.TotalItems - o.ScrapItems) / g.Sum(o => o.TotalItems),
            })
            .OrderBy(o => o.Key)
            .ToList();

        var list = new List<float>();

        foreach (var item in assetNumbers)
        {
            var assetQuery = oeeDataRepository.AsNoTracking()
            .Where(o => o.AssetNumber.StartsWith(item))
            .Where(o => o.Date >= model.Start && o.Date <= model.End);
            var temp = new List<float> {
                1f * query.Where(o => production.Contains(o.StatusNumber)).Sum(o=>o.Duration)/query.Where(o => !planned.Any(ps => ps == o.StatusNumber)).Sum(o => o.Duration),
                1f * query.Where(o => production.Contains(o.StatusNumber)).Sum(o => o.Duration) / query.Where(o => running.Contains(o.StatusNumber)).Sum(o => o.Duration),
                1f * query.Sum(o => o.TotalItems - o.ScrapItems) / query.Sum(o => o.TotalItems),
            };
            list.Add(temp[0] * temp[1] * temp[2]);
        }

        var componentsData = new List<float> {
            1f * query.Where(o => production.Contains(o.StatusNumber)).Sum(o=>o.Duration)/query.Where(o => !planned.Any(ps => ps == o.StatusNumber)).Sum(o => o.Duration),
            1f * query.Where(o => production.Contains(o.StatusNumber)).Sum(o => o.Duration) / query.Where(o => running.Contains(o.StatusNumber)).Sum(o => o.Duration),
            1f * query.Sum(o => o.TotalItems - o.ScrapItems) / query.Sum(o => o.TotalItems),
        };
        componentsData.Insert(0, componentsData[0] * componentsData[1] * componentsData[2]);

        var trendData = data
            .GroupBy(o => o.Date)
            .Select(g => new
            {
                g.Key,
                Availability = 1f * g.Where(o => production.Contains(o.StatusNumber)).Sum(o => o.Duration) / g.Where(o => !planned.Any(ps => ps == o.StatusNumber)).Sum(o => o.Duration),
                Performance = 1f * g.Where(o => production.Contains(o.StatusNumber)).Sum(o => o.Duration) / g.Where(o => running.Contains(o.StatusNumber)).Sum(o => o.Duration),
                Quality = 1f * g.Sum(o => o.TotalItems - o.ScrapItems) / g.Sum(o => o.TotalItems),
            })
            .OrderBy(o => o.Key)
            .ToList();

        var result = new OeeDashboardResult
        {
            Asset = new ChartModel
            {
                Title = new ChartTitle
                {
                    Text = "资产 OEE(%)"
                },
                XAxis = new ChartXAxis
                {
                    Data = assetList.Select(o => o.Name).OrderBy(o => o).ToList()
                },
                Series = new List<ChartSerie>
                {
                    new() {
                        Type ="bar",
                        Data = list //assetData.Select(o=>o.Availability*o.Performance*o.Quality).ToList(),
                    }
                }
            },
            Components = new ChartModel
            {
                Title = new ChartTitle
                {
                    Text = "OEE 组成(%)"
                },
                XAxis = new ChartXAxis
                {
                    Data = ["OEE", "可用性", "性能", "质量"]
                },
                Series = new List<ChartSerie>
                {
                    new() {
                        Type = "bar",
                        Data = componentsData
                    }
                }
            },
            Trend = new ChartModel
            {
                Title = new ChartTitle
                {
                    Text = "OEE 趋势(%)"
                },
                Legend = new ChartLegend
                {
                    Show = true,
                    Data = ["OEE", "可用性", "性能", "质量"],
                    Icon = "rect",
                    Left = "center",
                    Bottom = "0",
                },
                XAxis = new ChartXAxis
                {
                    Data = data.Select(o => o.Date).OrderBy(o => o).Select(o => o.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).Distinct().ToList()
                },
                Series = new List<ChartSerie>
                {
                    new() {
                        Name = "OEE",
                        Data = trendData.Select(o=>o.Availability*o.Performance*o.Quality).ToList(),
                        LineStyle = new ChartLineStyle
                        {
                            Color="green"
                        }
                    },
                    new() {
                        Name = "可用性",
                        Data = trendData.Select(o=>o.Availability).ToList(),
                        LineStyle = new ChartLineStyle
                        {
                            Color="yeallow"
                        }
                    },
                    new() {
                        Name = "性能",
                        Data = trendData.Select(o=>o.Performance).ToList(),
                        LineStyle = new ChartLineStyle
                        {
                            Color="blue"
                        }
                    },
                    new() {
                        Name = "质量",
                        Data = trendData.Select(o=>o.Quality).ToList(),
                        LineStyle = new ChartLineStyle
                        {
                            Color="red"
                        }
                    },
                }
            }
        };
        return Json(result);
    }
}
