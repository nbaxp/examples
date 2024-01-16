using Mapster;
using Wta.Application.Identity.Domain;
using Wta.Application.Identity.Models;
using Wta.Infrastructure.Application;
using Wta.Infrastructure.Attributes;

namespace Wta.Application.Identity.Mapper;

[Service<IMapper<Role, RoleModel>>(ServiceLifetime.Singleton)]
public class RoleMapper : DefaultMapper<Role, RoleModel>
{
    protected override void Config(TypeAdapterSetter<Role, RoleModel> toModel, TypeAdapterSetter<RoleModel, Role> toEntity)
    {
        toModel.Map(o => o.Permissions, o => o.RolePermissions.Select(o => o.PermissionId));
        toEntity.Map(o => o.RolePermissions, o => o.Permissions.Select(r => new RolePermission { PermissionId = r }));
    }
}
