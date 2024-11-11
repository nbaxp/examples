using Wta.Application.Iot.Domain;
using Wta.Infrastructure.Mapper;

namespace Wta.Application.Iot.Controllers;

public class IoTProductController(ILogger<IotProduct> logger, IStringLocalizer stringLocalizer, IObjerctMapper objectMapper, IRepository<IotProduct> repository, IEventPublisher eventPublisher, IExportImportService exportImportService) : GenericController<IotProduct, IotProduct>(logger, stringLocalizer, objectMapper, repository, eventPublisher, exportImportService)
{
    protected override IQueryable<IotProduct> Include(IQueryable<IotProduct> queryable)
    {
        return queryable.Include(o => o.ProductCapabilis);
    }
}
