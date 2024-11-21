using Wta.Application.Oee.Domain;
using Wta.Infrastructure.Mapper;

namespace Wta.Application.Oee.Controllers;
public class OeeDataController(ILogger<OeeData> logger, IStringLocalizer stringLocalizer, IObjerctMapper objectMapper, IRepository<OeeData> repository, IEventPublisher eventPublisher, IExportImportService exportImportService) : GenericController<OeeData, OeeData>(logger, stringLocalizer, objectMapper, repository, eventPublisher, exportImportService)
{
    protected override void ToEntity(OeeData entity, OeeData model, bool isCreate = false)
    {
        base.ToEntity(entity, model, isCreate);
        entity.Duration = (int)(entity.End - entity.Start).Value.TotalMinutes;
        if(entity.Total!=0)
        {
            entity.SpeedUpm = entity.Duration / entity.Total;
        }
    }
}
