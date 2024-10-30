using Wta.Application.Oee;
using Wta.Infrastructure.Mapper;

namespace Wta.Application.Platform.Controllers;

public class OeeConfigurationController(ILogger<OeeConfiguration> logger,
    IStringLocalizer stringLocalizer,
    IObjerctMapper mapper,
    IRepository<OeeConfiguration> repository,
    IEventPublisher eventPublisher,
    IExportImportService exportImportService) : GenericController<OeeConfiguration, OeeConfiguration>(logger, stringLocalizer, mapper, repository, eventPublisher, exportImportService)
{
    protected override void BeforeSave(OeeConfiguration entity)
    {
        if (entity.IsDefault)
        {
            Repository.Context.Set<OeeConfiguration>().Where(o => o.Id != entity.Id && o.IsDefault).ExecuteUpdate(o => o.SetProperty(b => b.IsDefault, p => false));
        }
        else
        {
            if (Repository.AsNoTracking().Any(o => o.Id != entity.Id && o.IsDefault))
            {
                throw new BadRequestException("没有默认的配置，通过设置当前项为默认配置可取消其他项的默认配置");
            }
        }
    }
}
