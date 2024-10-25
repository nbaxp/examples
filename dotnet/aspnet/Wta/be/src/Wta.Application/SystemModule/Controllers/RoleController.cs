using Wta.Infrastructure.Application.Models;
using Wta.Infrastructure.Mapper;

namespace Wta.Application.SystemModule.Controllers;

public class RoleController(ILogger<Role> logger, IStringLocalizer stringLocalizer, IObjerctMapper mapper, IRepository<Role> repository, IEventPublisher eventPublisher, IExportImportService exportImportService) : GenericController<Role, Role>(logger, stringLocalizer, mapper, repository, eventPublisher, exportImportService)
{
    [Authorize]
    public override ApiResult<QueryModel<Role>> Search(QueryModel<Role> model)
    {
        return base.Search(model);
    }

    protected override IQueryable<Role> Include(IQueryable<Role> queryable)
    {
        return queryable.Include(o => o.RolePermissions);
    }
}
