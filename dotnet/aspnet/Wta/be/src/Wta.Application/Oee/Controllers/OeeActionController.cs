using Wta.Application.Oee.Domain;
using Wta.Infrastructure.Mapper;

namespace Wta.Application.Platform.Controllers;

public class OeeActionController(ILogger<OeeAction> logger,
    IStringLocalizer stringLocalizer,
    IObjerctMapper mapper,
    IRepository<OeeAction> repository,
    IEventPublisher eventPublisher,
    IExportImportService exportImportService) : GenericController<OeeAction, OeeAction>(logger, stringLocalizer, mapper, repository, eventPublisher, exportImportService)
{
    protected override IQueryable<OeeAction> Include(IQueryable<OeeAction> queryable)
    {
        return queryable.Include(o => o.ActionRequirements);
    }
}
