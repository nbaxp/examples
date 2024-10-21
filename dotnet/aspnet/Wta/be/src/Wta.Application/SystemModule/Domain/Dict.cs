using Wta.Application.SystemModule.Data;

namespace Wta.Application.SystemModule.Domain;

[DependsOn<SystemDbContext>, SystemSettings, Display(Name = "字典", Order = 6)]
public class Dict : BaseTreeEntity<Dict>
{
}
