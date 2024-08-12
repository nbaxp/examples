
namespace Wta.Application.Default.Domain;

[OrganizationManagement, Display(Name = "部门", Order = 1)]
public class Department : BaseTreeEntity<Department>
{
    [KeyValue("hideForList", true)]
    [Hidden]
    public List<User> Users { get; set; } = new List<User>();
}
