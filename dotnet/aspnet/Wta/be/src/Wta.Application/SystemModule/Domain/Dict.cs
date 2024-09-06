using Wta.Application.SystemModule.Data;

namespace Wta.Application.SystemModule.Domain;

[SystemSettings, Display(Name = "字典", Order = 6)]
[DependsOn<SystemDbContext>]
public class Dict : BaseTreeEntity<Dict>
{
}
