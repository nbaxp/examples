using System.ComponentModel.DataAnnotations.Schema;
using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[Oee]
[DependsOn<PlatformDbContext>]
[Display(Name = "OEE事件分类", Order = 50)]
public class OeeActionCategory : BaseNameNumberEntity
{
}
