namespace Wta.Application.Platform.Data;

public partial class PlatformDbConfig : BaseDbConfig<PlatformDbContext>,
    IEntityTypeConfiguration<Audit>,
    IEntityTypeConfiguration<Tenant>,
    IEntityTypeConfiguration<Dict>,
    IEntityTypeConfiguration<Department>,
    IEntityTypeConfiguration<WorkGroup>,
    IEntityTypeConfiguration<WorkGroupUser>,
    IEntityTypeConfiguration<Post>,
    IEntityTypeConfiguration<User>,
    IEntityTypeConfiguration<Role>,
    IEntityTypeConfiguration<Permission>,
    IEntityTypeConfiguration<UserRole>,
    IEntityTypeConfiguration<RolePermission>,
    IEntityTypeConfiguration<UserLogin>,
    IEntityTypeConfiguration<ExternalApp>
{
    public void Configure(EntityTypeBuilder<Audit> builder)
    {
        builder.HasKey(o => o.Id);
        builder.HasIndex(o => o.EntityName);
        builder.HasIndex(o => o.EntityId);
    }

    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.Property(o => o.Name).IsRequired();
        builder.Property(o => o.Number).IsRequired();
        builder.HasIndex(o => o.Number).IsUnique();
    }

    public void Configure(EntityTypeBuilder<Dict> builder)
    {
    }

    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasOne(o => o.Manager).WithMany(o => o.Departments).HasForeignKey(o => o.ManagerId).OnDelete(DeleteBehavior.SetNull);
        //builder.Navigation(o => o.Users).AutoInclude();
    }

    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasOne(o => o.Department).WithMany(o => o.Posts).HasForeignKey(o => o.DepartmentId).OnDelete(DeleteBehavior.SetNull);
    }

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasOne(o => o.Department).WithMany(o => o.Users).HasForeignKey(o => o.DepartmentId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(o => o.Post).WithMany(o => o.Users).HasForeignKey(o => o.PostId).OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(o => new { o.TenantNumber, o.NormalizedUserName }).IsUnique();
        //builder.Navigation(o => o.UserRoles).AutoInclude();
    }

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasIndex(o => new { o.TenantNumber, o.Number }).IsUnique();
        //builder.Navigation(o => o.RolePermissions).AutoInclude();
    }

    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(o => new { o.UserId, o.RoleId });
        builder.HasOne(o => o.User).WithMany(o => o.UserRoles).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Role).WithMany(o => o.UserRoles).HasForeignKey(o => o.RoleId).OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<Permission> builder)
    {
    }

    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(o => new { o.RoleId, o.PermissionId });
        builder.HasOne(o => o.Role).WithMany(o => o.RolePermissions).HasForeignKey(o => o.RoleId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Permission).WithMany(o => o.RolePermissions).HasForeignKey(o => o.PermissionId).OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<WorkGroup> builder)
    {
        //builder.Navigation(o => o.WorkGroupUsers).AutoInclude();
    }

    public void Configure(EntityTypeBuilder<WorkGroupUser> builder)
    {
        builder.HasKey(o => new { o.WorkGroupId, o.UserId });
        builder.HasOne(o => o.WorkGroup).WithMany(o => o.WorkGroupUsers).HasForeignKey(o => o.WorkGroupId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.User).WithMany(o => o.WorkGroupUsers).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<ExternalApp> builder)
    {
        builder.HasIndex(o => new { o.TenantNumber, o.Name }).IsUnique();
        builder.HasOne(o => o.User).WithMany(o => o.Apps).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.HasOne(o => o.User).WithMany(o => o.UserLogins).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
