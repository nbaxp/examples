using System.ComponentModel.DataAnnotations.Schema;
using Wta.Application.Platform.Data;

namespace Wta.Application.Platform.Domain;

[OrganizationManagement, Display(Name = "部门", Order = 1)]
[DependsOn<PlatformDbContext>]
public class Department : BaseTreeEntity<Department>, IEntityTypeConfiguration<Department>
{
    [UIHint("select")]
    [KeyValue("url", "user/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "负责人")]
    public Guid? ManagerId { get; set; }

    [Hidden]
    public User? Manager { get; set; }

    [KeyValue("hideForList", true)]
    [Hidden]
    [IgnoreToModel]
    public List<User> Users { get; set; } = [];

    [KeyValue("hideForList", true)]
    [Hidden]
    public List<Post> Posts { get; set; } = [];

    [UIHint("select")]
    [KeyValue("url", "user/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("hideForList", true)]
    //[KeyValue("hideForEdit", true)]
    [Display(Name = "成员")]
    [NotMapped]
    public List<Guid> DepartmentUsers { get; set; } = null!;

    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasOne(o => o.Manager).WithMany(o => o.Departments).HasForeignKey(o => o.ManagerId).OnDelete(DeleteBehavior.SetNull);
    }
    //{
    //    get
    //    {
    //        return this._users??(this._users == this.Users?.Select(o => o.Id).ToList());
    //    }
    //    //set {
    //    //    this.Users = value.Select(o => new User { Id = o }).ToList();
    //    //}
    //}
}
