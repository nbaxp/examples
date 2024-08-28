using Wta.Infrastructure.Mapper;

namespace Wta.Application.WmsModule.Controllers;

public class InventoryController(ILogger<Inventory> logger, IStringLocalizer stringLocalizer, IObjerctMapper objectMapper, IRepository<Inventory> repository, IEventPublisher eventPublisher, IExportImportService exportImportService)
    : GenericController<Inventory, Inventory>(logger, stringLocalizer, objectMapper, repository, eventPublisher, exportImportService)
{
    [Ignore]
    public override ApiResult<bool> Create([FromBody] Inventory model)
    {
        throw new NotImplementedException();
    }

    [Ignore]
    public override ApiResult<bool> Update([FromBody] Inventory model)
    {
        throw new NotImplementedException();
    }

    [Ignore]
    public override ApiResult<bool> Delete([FromBody] Guid[] items)
    {
        throw new NotImplementedException();
    }

    [Display(Name = "出库入库")]
    [Button(Type = ButtonType.Table)]
    public virtual ApiResult<bool> OutIn([FromBody] Guid id)
    {
        return Json(true);
    }

    [Display(Name = "库存调拨")]
    [Button(Type = ButtonType.Table)]
    public virtual ApiResult<bool> Transfer([FromBody] Guid id)
    {
        return Json(true);
    }
}
