using Wta.Infrastructure.Mapper;

namespace Wta.Application.SystemModule.Controllers;

public class ExternalAppController(ILogger<ExternalApp> logger,
    IStringLocalizer stringLocalizer,
    IObjerctMapper mapper,
    IRepository<ExternalApp> repository,
    IRepository<User> userRepository,
    IEventPublisher eventPublisher,
    IExportImportService exportImportService) : GenericController<ExternalApp, ExternalApp>(logger, stringLocalizer, mapper, repository, eventPublisher, exportImportService)
{
    protected override void ToEntity(ExternalApp entity, ExternalApp model, bool isCreate = false)
    {
        if (isCreate)
        {
            var normalizedUserName = User.Identity?.Name?.ToUpperInvariant()!;
            entity.UserId = userRepository.AsNoTracking().First(o => o.NormalizedUserName == normalizedUserName).Id;
        }
    }
}
