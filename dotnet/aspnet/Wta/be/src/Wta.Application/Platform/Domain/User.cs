using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Wta.Application.Platform.Domain;

[PermissionManagement, Display(Name = "用户", Order = 3)]
[DependsOn<PlatformDbContext>]
public class User : Entity, IEntityTypeConfiguration<User>
{
    [ReadOnly(true)]
    [Display(Name = "登录名")]
    public string UserName { get; set; } = default!;

    [KeyValue("hideForList", true)]
    [KeyValue("hideForQuery", true)]
    [KeyValue("placeholder", "保持为空则不更新")]
    [DataType(DataType.Password)]
    [Display(Name = "密码")]
    [NotMapped]
    public string? Password { get; set; }

    [Display(Name = "名称")]
    public string Name { get; set; } = default!;

    [Display(Name = "性别")]
    [UIHint("radio")]
    public Sex Sex { get; set; }

    [Display(Name = "出生日期")]
    [DataType(DataType.Date)]
    public DateTime? Birthday { get; set; }

    [UIHint("image-upload")]
    [KeyValue("accept", ".svg,.png")]
    [KeyValue("url", "file/upload")]
    [Display(Name = "头像")]
    public string? Avatar { get; set; }

    [Hidden]
    public string NormalizedUserName { get; set; } = default!;

    [Display(Name = "邮箱")]
    public string? Email { get; set; }

    [Hidden]
    public string? NormalizedEmail { get; set; }

    [Display(Name = "邮箱已确认")]
    public bool EmailConfirmed { get; set; }

    [Hidden]
    [ReadOnly(true), IgnoreToModel]
    public string? PasswordHash { get; set; }

    [Hidden]
    [ValidateNever]
    [ReadOnly(true), IgnoreToModel]
    public string SecurityStamp { get; set; } = default!;

    [Display(Name = "手机号")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "手机号确认")]
    public bool PhoneNumberConfirmed { get; set; }

    [Display(Name = "双因子验证")]
    public bool TwoFactorEnabled { get; set; }

    [Display(Name = "登录失败")]
    public int AccessFailedCount { get; set; }

    [Display(Name = "解锁时间")]
    public DateTime? LockoutEnd { get; set; }

    [Display(Name = "启用锁定")]
    public bool LockoutEnabled { get; set; }

    [Display(Name = "系统用户")]
    public bool IsReadOnly { get; set; }

    [UIHint("select")]
    [KeyValue("url", "department/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "部门")]
    public Guid? DepartmentId { get; set; }

    [Hidden]
    public Department? Department { get; set; }

    [UIHint("select")]
    [KeyValue("url", "post/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "岗位")] public Guid? PostId { get; set; }

    [Hidden]
    public Post? Post { get; set; }

    [Hidden]
    [JsonIgnore]
    public List<UserRole> UserRoles { get; set; } = [];

    [Hidden]
    public List<WorkGroupUser> WorkGroupUsers { get; set; } = [];

    [Hidden]
    public List<Department> Departments { get; set; } = [];

    [UIHint("select")]
    [KeyValue("url", "role/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("skipSorting", true)]
    [NotMapped]
    public List<Guid> Roles
    {
        get
        {
            return this.UserRoles?.Select(o => o.RoleId).ToList() ?? [];
        }
        set
        {
            this.UserRoles = value?.Distinct().Select(o => new UserRole { RoleId = o }).ToList() ?? [];
        }
    }

    [Hidden]
    public List<UserLogin> UserLogins { get; set; } = [];

    [Hidden]
    public List<ExternalApp> Apps { get; set; } = [];

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasOne(o => o.Department).WithMany(o => o.Users).HasForeignKey(o => o.DepartmentId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(o => o.Post).WithMany(o => o.Users).HasForeignKey(o => o.PostId).OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(o => new { o.TenantNumber, o.NormalizedUserName }).IsUnique();
        //builder.Navigation(o => o.UserRoles).AutoInclude();
    }
}
