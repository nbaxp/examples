using Wta.Application.BaseData.Domain;
using Wta.Infrastructure.Mapper;

namespace Wta.Application.Platform.Controllers;

public class WorkstationController(ILogger<Workstation> logger,
    IStringLocalizer stringLocalizer,
    IObjerctMapper mapper,
    IRepository<Workstation> repository,
    IEventPublisher eventPublisher,
    IExportImportService exportImportService) : GenericController<Workstation, Workstation>(logger, stringLocalizer, mapper, repository, eventPublisher, exportImportService)
{
    protected override IQueryable<Workstation> Include(IQueryable<Workstation> queryable)
    {
        return queryable.Include(o => o.WorkstationDevices);
    }
}
