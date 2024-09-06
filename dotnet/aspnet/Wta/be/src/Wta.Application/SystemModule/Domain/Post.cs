using Wta.Application.SystemModule.Data;

namespace Wta.Application.SystemModule.Domain;

[OrganizationManagement]
[Display(Name = "岗位", Order = 2)]
[DependsOn<SystemDbContext>]
public class Post : BaseTreeEntity<Post>
{
    [UIHint("select")]
    [KeyValue("url", "department/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "部门")] public Guid? DepartmentId { get; set; }

    public Department? Department { get; set; }

    [Hidden]
    public List<User> Users { get; set; } = [];

    [Hidden]
    public List<Department> Departments { get; set; } = [];
}
