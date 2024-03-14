using Wta.Application.Default.Attributes;
using Wta.Infrastructure.Domain;

namespace Wta.Application.Default.Domain;

[SystemManagement, Display(Order = 1)]
public class Dict : BaseTreeEntity<Dict>
{
}
