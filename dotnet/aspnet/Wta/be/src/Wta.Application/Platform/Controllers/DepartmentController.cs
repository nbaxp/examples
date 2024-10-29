using Wta.Infrastructure.Application.Models;
using Wta.Infrastructure.Mapper;

namespace Wta.Application.Platform.Controllers;

public class DepartmentController(ILogger<Department> logger,
    IStringLocalizer stringLocalizer,
    IObjerctMapper mapper,
    IRepository<Department> repository,
    IRepository<User> userRepository,
    IEventPublisher eventPublisher,
    IExportImportService exportImportService) : GenericController<Department, Department>(logger, stringLocalizer, mapper, repository, eventPublisher, exportImportService)
{
    [Authorize]
    public override ApiResult<QueryModel<Department>> Search(QueryModel<Department> model)
    {
        return base.Search(model);
    }

    protected override IQueryable<Department> Include(IQueryable<Department> queryable)
    {
        return queryable.Include(o => o.Users);
    }

    protected override void ToModel(Department entity, Department model)
    {
        model.DepartmentUsers = entity.Users.Select(x => x.Id).ToList();
    }

    protected override void ToEntity(Department entity, Department model, bool isCreate = false)
    {
        entity.Users = [.. userRepository.Query().Where(o => model.DepartmentUsers.Contains(o.Id))];
    }
}
