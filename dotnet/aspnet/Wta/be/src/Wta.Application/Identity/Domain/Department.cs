using Wta.Application.Identity.Attributes;
using Wta.Infrastructure.Domain;

namespace Wta.Application.Identity.Domain;

[SystemManagement, Display(Order = 2)]
public class Department : BaseTreeEntity<Department>
{
    public List<User> Users { get; set; } = new List<User>();
}
