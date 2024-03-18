using Wta.Infrastructure.Application.Domain;

namespace Wta.Application.Default.Domain;

[SystemManagement, Display(Order = 2)]
public class Department : BaseTreeEntity<Department>
{
    public List<User> Users { get; set; } = new List<User>();
}
