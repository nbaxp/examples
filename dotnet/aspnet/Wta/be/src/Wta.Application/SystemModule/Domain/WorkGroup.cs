using System.ComponentModel.DataAnnotations.Schema;
using Wta.Application.SystemModule.Data;

namespace Wta.Application.SystemModule.Domain;

[OrganizationManagement, Display(Name = "工作组", Order = 3)]
[DependsOn<SystemDbContext>]
public class WorkGroup : BaseTreeEntity<WorkGroup>
{
    [UIHint("select")]
    [KeyValue("url", "user/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "负责人")]
    public Guid ManagerId { get; set; }

    [Hidden]
    public List<WorkGroupUser> WorkGroupUsers { get; set; } = [];

    [KeyValue("hideForList", true)]
    [UIHint("select")]
    [KeyValue("url", "user/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [NotMapped]
    [Display(Name = "成员")]
    public List<Guid> Users
    {
        get
        {
            return [.. WorkGroupUsers.Select(o => o.UserId)];
        }
        set
        {
            WorkGroupUsers.Clear();
            WorkGroupUsers.AddRange(value.Select(o => new WorkGroupUser { UserId = o }));
        }
    }
}
