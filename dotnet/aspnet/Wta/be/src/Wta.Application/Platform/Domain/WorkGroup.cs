using System.ComponentModel.DataAnnotations.Schema;
using Wta.Application.Platform.Data;

namespace Wta.Application.Platform.Domain;

[OrganizationManagement, Display(Name = "工作组", Order = 3)]
[DependsOn<PlatformDbContext>]
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
            return this.WorkGroupUsers?.Select(o => o.UserId).ToList() ?? [];
        }
        set
        {
            this.WorkGroupUsers = value?.Distinct().Select(o => new WorkGroupUser { UserId = o }).ToList() ?? [];
        }
    }
}
