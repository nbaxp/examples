using Wta.Application.Iot.Domain;
using Wta.Infrastructure.Mapper;

namespace Wta.Application.Iot.Controllers;

public class IotDeviceController(ILogger<IotDevice> logger, IStringLocalizer stringLocalizer, IObjerctMapper objectMapper, IRepository<IotDevice> repository, IEventPublisher eventPublisher, IExportImportService exportImportService) : GenericController<IotDevice, IotDevice>(logger, stringLocalizer, objectMapper, repository, eventPublisher, exportImportService)
{
    protected override IQueryable<IotDevice> Include(IQueryable<IotDevice> queryable)
    {
        return queryable
            .Include(o => o.Product)
            .ThenInclude(o => o!.ProductCapabilis)
            .ThenInclude(o => o.Capabilit);
    }
}
