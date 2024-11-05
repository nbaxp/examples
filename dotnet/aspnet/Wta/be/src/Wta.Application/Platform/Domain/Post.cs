namespace Wta.Application.Platform.Domain;

[OrganizationManagement]
[Display(Name = "岗位", Order = 2)]
[DependsOn<PlatformDbContext>]
public class Post : BaseTreeEntity<Post>, IEntityTypeConfiguration<Post>
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

    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasOne(o => o.Department).WithMany(o => o.Posts).HasForeignKey(o => o.DepartmentId).OnDelete(DeleteBehavior.SetNull);
    }
}
