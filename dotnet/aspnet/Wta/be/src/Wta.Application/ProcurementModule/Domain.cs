using Wta.Application.SystemModule.Data;

namespace Wta.Application.ProcurementModule;

[Procurement]
[Display(Name = "采购订单")]
[DependsOn<SystemDbContext>]
public class Test : Entity
{
}
