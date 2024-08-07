namespace Wta.Application.Default.Domain;

[SystemManagement, Display(Name = "部门", Order = 1)]
public class Department : BaseTreeEntity<Department>
{
    [KeyValue("hideForList", true)]
    public List<User> Users { get; set; } = new List<User>();
}
