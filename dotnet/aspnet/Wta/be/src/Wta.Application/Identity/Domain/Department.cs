using Wta.Infrastructure.Domain;

namespace Wta.Application.Identity.Domain;

[IdentityGroup, Display(Order = 1)]
public class Department : BaseTreeEntity<Department>
{
    public List<User> Users { get; set; } = new List<User>();
}
