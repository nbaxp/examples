using Wta.Infrastructure.Application.Models;
using Wta.Infrastructure.Mapper;

namespace Wta.Application.SystemModule.Controllers;

public class WorkGroupController(ILogger<WorkGroup> logger, IStringLocalizer stringLocalizer, IObjerctMapper mapper, IRepository<WorkGroup> repository, IEventPublisher eventPublisher, IExportImportService exportImportService) : GenericController<WorkGroup, WorkGroup>(logger, stringLocalizer, mapper, repository, eventPublisher, exportImportService)
{
    [Authorize]
    public override ApiResult<QueryModel<WorkGroup>> Search(QueryModel<WorkGroup> model)
    {
        return base.Search(model);
    }

    protected override IQueryable<WorkGroup> Include(IQueryable<WorkGroup> queryable)
    {
        return queryable.Include(o => o.WorkGroupUsers);
    }
}
