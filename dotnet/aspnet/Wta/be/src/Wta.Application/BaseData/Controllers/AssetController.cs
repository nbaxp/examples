using Wta.Application.BaseData.Domain;
using Wta.Infrastructure.Mapper;

namespace Wta.Application.Platform.Controllers;

public class AssetController(ILogger<Asset> logger,
    IStringLocalizer stringLocalizer,
    IObjerctMapper mapper,
    IRepository<Asset> repository,
    IEventPublisher eventPublisher,
    IExportImportService exportImportService) : GenericController<Asset, Asset>(logger, stringLocalizer, mapper, repository, eventPublisher, exportImportService)
{
    protected override IQueryable<Asset> Include(IQueryable<Asset> queryable)
    {
        return queryable.Where(o => o.Values.Any(o => o.Key == "key" && o.Value == "value"));
    }
}
