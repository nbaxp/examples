using Wta.Application.Identity.Domain;
using Wta.Application.Identity.Models;
using Wta.Infrastructure.Interfaces;
using Wta.Infrastructure.Services;

namespace Wta.Application.Identity.Mapper;

public class IdentityrMapper : IMapperConfig<User, UserModel>,
    IMapperConfig<UserModel, User>,
    IMapperConfig<Role, RoleModel>,
    IMapperConfig<RoleModel, Role>
{
    public void Config(MapperConfigBuilder<User, UserModel> builder)
    {
        builder.Map(o => o.Roles, o => o.UserRoles.Select(o => o.RoleId).ToList());
    }

    public void Config(MapperConfigBuilder<UserModel, User> builder)
    {
        //builder.Map(o => o.NormalizedUserName, o => o.UserName!.ToUpperInvariant());
        //builder.Map(o => o.UserRoles, o => o.Roles.Select(p => new UserRole { RoleId = p }).ToList());
    }

    public void Config(MapperConfigBuilder<Role, RoleModel> builder)
    {
        builder.Map(o => o.Permissions, o => o.RolePermissions.Select(o => o.PermissionId).ToList());
    }

    public void Config(MapperConfigBuilder<RoleModel, Role> builder)
    {
        builder.Map(o => o.RolePermissions, o => o.Permissions.Select(p => new RolePermission { PermissionId = p }).ToList());
    }
}
