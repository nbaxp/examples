namespace Wta.Application.Default.Domain;

[SystemManagement, Display(Name = "部门", Order = 2)]
public class Department : BaseTreeEntity<Department>
{
    [Hidden]
    public List<User> Users { get; set; } = new List<User>();
}
