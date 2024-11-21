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
    public ApiResult<object> Index(OeeDashboard model)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException();
        }
        //资产编号前缀
        var assetPath = oeeAssetRepository.AsNoTracking().Where(o => o.Number == model.AssetNumber).Select(o => o.Path).FirstOrDefault() ?? "/";
        //资产列表
        var assetList = oeeAssetRepository.AsNoTracking().Where(o => o.Path.StartsWith(assetPath)).ToList();
        //资产编号列表
        var assetNumbers = assetList.Select(o => o.Number).ToList();
        //状态列表，包含类型预加载
        var assetStatusList = oeeStatusRepository.AsNoTracking().Include(o => o.Type).ToList();
        //正常生产
        var production = assetStatusList.Where(o => o.Type.Name == "正常生产").Select(o => o.Number).ToList();
        //正常生产+非计划停产（设备跳转+设备故障+非设备因素）=全部时间-计划停产
        var planned = assetStatusList.Where(o => o.Type.Name != "计划停产" && o.Type.Name != "非设备因素停产").Select(o => o.Number).ToList();

        var query = oeeDataRepository
            .AsNoTracking()
            .Where(o => o.Date >= model.Start && o.Date <= model.End)
            .Where(o => assetNumbers.Contains(o.AssetNumber));

        var dataGroup = query
            .GroupBy(o => new
            {
                o.AssetNumber,
                o.Date,
                StatusNumber = o.Status.Number,
                o.Duration,
                o.Total,
                o.EequipmentScrap,
            })
            .Select(g => new OeeDayModel
            {
                Date = g.Key.Date,
                AssetNumber = g.Key.AssetNumber,
                StatusNumber = g.Key.StatusNumber,
                Duration = g.Sum(o => o.Duration),
                StandardUpm = g.Average(o => o.StandardUpm),
                Total = g.Sum(o => o.Total),
                EequipmentScrap = g.Sum(o => o.EequipmentScrap),
                NonEequipmentScrap = g.Sum(o => o.NonEequipmentScrap),
            })
            .ToList();

        var assetData = dataGroup
            .GroupBy(o => o.AssetNumber)
            .Select(o => new OeeGroupModel
            {
                Key = o.Key,
                Availability = 1f * o.Where(o => production.Contains(o.StatusNumber)).Sum(o => o.Duration) / o.Where(o => planned.Contains(o.StatusNumber)).Sum(o => o.Duration),
                Performance = 1f * o.Sum(o => o.Total) * o.Where(o => production.Contains(o.StatusNumber)).Average(o => o.StandardUpm) / o.Where(o => production.Contains(o.StatusNumber)).Sum(o => o.Duration),
                Quality = 1f * o.Sum(o => o.Total - o.EequipmentScrap - o.NonEequipmentScrap) / o.Sum(o => o.Total - o.NonEequipmentScrap),
            })
            .OrderBy(o => o.Key)
            .ToList();

        var list = new List<float>();

        foreach (var item in assetNumbers)
        {
            var assetQuery = oeeDataRepository
                .AsNoTracking()
                .Where(o => o.Date >= model.Start && o.Date <= model.End)
                .Where(o => o.AssetNumber.StartsWith(item));
            var oeeModel = new OeeModel
            {
                Availability = 1f * assetQuery.Where(o => production.Contains(o.Status.Number)).Sum(o => o.Duration) / assetQuery.Where(o => planned.Contains(o.Status.Number)).Sum(o => o.Duration),
                Performance = 1f * assetQuery.Sum(o => o.Total) * assetQuery.Where(o => production.Contains(o.Status.Number)).Average(o => o.StandardUpm) / assetQuery.Where(o => production.Contains(o.Status.Number)).Sum(o => o.Duration),
                Quality = 1f * assetQuery.Sum(o => o.Total - o.EequipmentScrap - o.NonEequipmentScrap) / assetQuery.Sum(o => o.Total - o.NonEequipmentScrap),
            };
            list.Add(oeeModel.Availability * oeeModel.Performance * oeeModel.Quality);
        }

        var componentsData = new List<float> {
            1f * query.Where(o => production.Contains(o.Status.Number)).Sum(o=>o.Duration)/query.Where(o => planned.Contains(o.Status.Number)).Sum(o => o.Duration),
            1f * query.Sum(o => o.Total) * query.Where(o => production.Contains(o.Status.Number)).Average(o=>o.StandardUpm) / query.Where(o => production.Contains(o.Status.Number)).Sum(o => o.Duration),
            1f * query.Sum(o => o.Total - o.EequipmentScrap - o.NonEequipmentScrap) / query.Sum(o => o.Total- o.NonEequipmentScrap),
        };
        componentsData.Insert(0, componentsData[0] * componentsData[1] * componentsData[2]);

        var trendData = dataGroup
            .GroupBy(o => o.Date)
            .Select(g => new
            {
                g.Key,
                Availability = 1f * g.Where(o => production.Contains(o.StatusNumber)).Sum(o => o.Duration) / g.Where(o => planned.Contains(o.StatusNumber)).Sum(o => o.Duration),
                Performance = 1f * g.Sum(o => o.Total) * g.Where(o => production.Contains(o.StatusNumber)).Average(o => o.StandardUpm) / g.Where(o => production.Contains(o.StatusNumber)).Sum(o => o.Duration),
                Quality = 1f * g.Sum(o => o.Total - o.EequipmentScrap - o.NonEequipmentScrap) / g.Sum(o => o.Total - o.NonEequipmentScrap),
            })
            .OrderBy(o => o.Key)
            .ToList();

        var result = new
        {
            Chart11 = new
            {
                Title = new
                {
                    Text = "资产 OEE(%)"
                },
                XAxis = new
                {
                    Data = assetList.Select(o => o.Name).OrderBy(o => o).ToList()
                },
                Series = new List<object>
                {
                    new {
                        Type ="bar",
                        Data = list
                    }
                }
            },
            Chart12 = new
            {
                Title = new
                {
                    Text = "OEE 组成(%)"
                },
                XAxis = new
                {
                    Data = new List<string> { "OEE", "可用性", "性能", "质量" }
                },
                Series = new List<object>
                {
                    new {
                        Type = "bar",
                        Data = componentsData
                    }
                }
            },
            Chart13 = new
            {
                Title = new
                {
                    Text = "OEE 趋势(%)"
                },
                Legend = new
                {
                    Data = new List<string> { "OEE", "可用性", "性能", "质量" },
                },
                XAxis = new
                {
                    Data = dataGroup.Select(o => o.Date).OrderBy(o => o).Select(o => o.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).Distinct().ToList()
                },
                Series = new List<object>
                {
                    new {
                        Type = "line",
                        Name = "OEE",
                        Data = trendData.Select(o=>o.Availability*o.Performance*o.Quality).ToList(),
                        LineStyle = new
                        {
                            Color="green"
                        }
                    },
                    new {
                        Type = "line",
                        Name = "可用性",
                        Data = trendData.Select(o=>o.Availability).ToList(),
                        LineStyle = new
                        {
                            Color="yeallow"
                        }
                    },
                    new {
                        Type = "line",
                        Name = "性能",
                        Data = trendData.Select(o=>o.Performance).ToList(),
                        LineStyle = new
                        {
                            Color="blue"
                        }
                    },
                    new {
                        Type = "line",
                        Name = "质量",
                        Data = trendData.Select(o=>o.Quality).ToList(),
                        LineStyle = new
                        {
                            Color="red"
                        }
                    },
                }
            }
        };
        return Json<object>(result);
    }

    //[Display(Name = "OEE可用性")]
    //[Authorize]
    //[HttpPost]
    //[Ignore]
    //public ApiResult<object> Availability(OeeDashboard model)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        throw new BadRequestException();
    //    }
    //    var assetPath = oeeAssetRepository.AsNoTracking().Where(o => o.Number == model.AssetNumber).Select(o => o.Path).FirstOrDefault() ?? "/";
    //    var assetList = oeeAssetRepository.AsNoTracking().Where(o => o.Path.StartsWith(assetPath)).ToList();
    //    var assetNumbers = assetList.Select(o => o.Number).ToList();
    //    var assetStatusList = oeeStatusRepository.AsNoTracking().ToList();
    //    var planned = assetStatusList.Where(o => o.Type == OeeStatusType.PlannedDowntime).Select(o => o.Number).ToList();
    //    var production = assetStatusList.Where(o => o.Type == OeeStatusType.ProductionTime).Select(o => o.Number).ToList();
    //    var running = assetStatusList.Where(o => o.Type == OeeStatusType.ProductionTime || o.Type == OeeStatusType.UnloadedTime).Select(o => o.Number).ToList();

    //    var query = oeeDataRepository.AsNoTracking()
    //        .Where(o => assetNumbers.Contains(o.AssetNumber))
    //        .Where(o => o.Date >= model.Start && o.Date <= model.End);

    //    var data = query
    //        .GroupBy(o => new
    //        {
    //            o.AssetNumber,
    //            o.Date,
    //            o.StatusNumber,
    //            o.Duration,
    //            o.Total,
    //            o.EequipmentScrap,
    //        })
    //        .Select(g => new
    //        {
    //            g.Key.AssetNumber,
    //            g.Key.Date,
    //            g.Key.StatusNumber,
    //            Duration = g.Sum(o => o.Duration),
    //            TotalItems = g.Sum(o => o.TotalItems),
    //            ScrapItems = g.Sum(o => o.ScrapItems),
    //        })
    //        .ToList();

    //    var assetData = data
    //        .GroupBy(o => o.AssetNumber)
    //        .Select(g => new
    //        {
    //            g.Key,
    //            Availability = 1f * g.Where(o => production.Contains(o.StatusNumber)).Sum(o => o.Duration) / g.Where(o => !planned.Any(ps => ps == o.StatusNumber)).Sum(o => o.Duration),
    //            Performance = 1f * g.Where(o => production.Contains(o.StatusNumber)).Sum(o => o.Duration) / g.Where(o => running.Contains(o.StatusNumber)).Sum(o => o.Duration),
    //            Quality = 1f * g.Sum(o => o.TotalItems - o.ScrapItems) / g.Sum(o => o.TotalItems),
    //        })
    //        .OrderBy(o => o.Key)
    //        .ToList();

    //    var data1 = new List<float>();

    //    foreach (var item in assetNumbers)
    //    {
    //        var assetQuery = oeeDataRepository.AsNoTracking()
    //        .Where(o => o.AssetNumber.StartsWith(item))
    //        .Where(o => o.Date >= model.Start && o.Date <= model.End);
    //        var temp = new List<float> {
    //            1f * query.Where(o => production.Contains(o.StatusNumber)).Sum(o=>o.Duration)/query.Where(o => !planned.Any(ps => ps == o.StatusNumber)).Sum(o => o.Duration),
    //        };
    //        data1.Add(temp[0]);
    //    }

    //    var data2 = query
    //        .GroupBy(o => o.StatusNumber)
    //        .Select(g => new
    //        {
    //            Key = g.Key,
    //            Value = g.Sum(o => o.Duration)
    //        })
    //        .OrderByDescending(o => o.Value)
    //        .ToList()
    //        .Select(o => new
    //        {
    //            Key = assetStatusList.First(p => p.Number == o.Key).Name,
    //            o.Value
    //        })
    //        .ToList();
    //    var trendData = data
    //        .GroupBy(o => o.Date)
    //        .Select(g => new
    //        {
    //            g.Key,
    //            Availability = 1f * g.Where(o => production.Contains(o.StatusNumber)).Sum(o => o.Duration) / g.Where(o => !planned.Any(ps => ps == o.StatusNumber)).Sum(o => o.Duration),
    //            Performance = 1f * g.Where(o => production.Contains(o.StatusNumber)).Sum(o => o.Duration) / g.Where(o => running.Contains(o.StatusNumber)).Sum(o => o.Duration),
    //            Quality = 1f * g.Sum(o => o.TotalItems - o.ScrapItems) / g.Sum(o => o.TotalItems),
    //        })
    //        .OrderBy(o => o.Key)
    //        .ToList();

    //    var result = new
    //    {
    //        Chart1 = new ChartModel
    //        {
    //            Title = new ChartTitle
    //            {
    //                Text = "资产可用性(%)"
    //            },
    //            XAxis = new ChartXAxis
    //            {
    //                Data = assetList.Select(o => o.Name).OrderBy(o => o).ToList()
    //            },
    //            Series = new List<ChartSerie>
    //            {
    //                new() {
    //                    Type ="bar",
    //                    Data = data1 //assetData.Select(o=>o.Availability*o.Performance*o.Quality).ToList(),
    //                }
    //            }
    //        },
    //        Chart2 = new ChartModel
    //        {
    //            Title = new ChartTitle
    //            {
    //                Text = "状态(分钟)"
    //            },
    //            XAxis = new ChartXAxis
    //            {
    //                Data = data2.Select(o => o.Key).ToList()
    //            },
    //            Series = new List<ChartSerie>
    //            {
    //                new() {
    //                    Type = "bar",
    //                    Data = data2.Select(o=>(float)o.Value).ToList()
    //                }
    //            }
    //        },
    //        Chart3 = new ChartModel
    //        {
    //            Title = new ChartTitle
    //            {
    //                Text = "可用性趋势(分钟)"
    //            },
    //            Legend = new ChartLegend
    //            {
    //                Data = ["OEE", "可用性", "性能", "质量"],
    //                Icon = "rect",
    //                Left = "center",
    //                Bottom = "0",
    //            },
    //            XAxis = new ChartXAxis
    //            {
    //                AxisLabel = new ChartAxisLabel
    //                {
    //                    Rotate = 0
    //                },
    //                Data = data.Select(o => o.Date).OrderBy(o => o).Select(o => o.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).Distinct().ToList()
    //            },
    //            Series = new List<ChartSerie>
    //            {
    //                new() {
    //                    Name = "OEE",
    //                    Data = trendData.Select(o=>o.Availability*o.Performance*o.Quality).ToList(),
    //                    LineStyle = new ChartLineStyle
    //                    {
    //                        Color="green"
    //                    }
    //                },
    //                new() {
    //                    Name = "可用性",
    //                    Data = trendData.Select(o=>o.Availability).ToList(),
    //                    LineStyle = new ChartLineStyle
    //                    {
    //                        Color="yeallow"
    //                    }
    //                },
    //                new() {
    //                    Name = "性能",
    //                    Data = trendData.Select(o=>o.Performance).ToList(),
    //                    LineStyle = new ChartLineStyle
    //                    {
    //                        Color="blue"
    //                    }
    //                },
    //                new() {
    //                    Name = "质量",
    //                    Data = trendData.Select(o=>o.Quality).ToList(),
    //                    LineStyle = new ChartLineStyle
    //                    {
    //                        Color="red"
    //                    }
    //                },
    //            }
    //        }
    //    };
    //    return Json<object>(result);
    //}
}
