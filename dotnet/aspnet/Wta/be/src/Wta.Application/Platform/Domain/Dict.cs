using Wta.Application.Platform.Data;

namespace Wta.Application.Platform.Domain;

[DependsOn<PlatformDbContext>, SystemSettings, Display(Name = "字典", Order = 6)]
public class Dict : BaseTreeEntity<Dict>
{
}
