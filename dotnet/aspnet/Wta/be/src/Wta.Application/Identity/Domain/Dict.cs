using Wta.Application.Identity.Attributes;
using Wta.Infrastructure.Domain;

namespace Wta.Application.Identity.Domain;

[SystemManagement, Display(Order = 1)]
public class Dict : BaseTreeEntity<Dict>
{
}
