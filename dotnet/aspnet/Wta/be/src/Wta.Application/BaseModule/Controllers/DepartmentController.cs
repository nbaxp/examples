using Wta.Infrastructure.Mapper;

namespace Wta.Application.BaseModule.Controllers;

public class DepartmentController(ILogger<Department> logger,
    IStringLocalizer stringLocalizer,
    IObjerctMapper mapper,
    IRepository<Department> repository,
    IRepository<User> userRepository,
    IEventPublisher eventPublisher,
    IExportImportService exportImportService) : GenericController<Department, Department>(logger, stringLocalizer, mapper, repository, eventPublisher, exportImportService)
{
    protected override void ToModel(Department entity, Department model)
    {
        model.DepartmentUsers = entity.Users.Select(x => x.Id).ToList();
    }

    protected override void ToEntity(Department entity, Department model, bool isCreate)
    {
        entity.Users = [.. userRepository.Query().Where(o => model.DepartmentUsers.Contains(o.Id))];
    }
}
