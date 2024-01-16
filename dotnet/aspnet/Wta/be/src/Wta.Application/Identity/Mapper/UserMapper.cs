using Mapster;
using Wta.Application.Identity.Domain;
using Wta.Application.Identity.Models;
using Wta.Infrastructure.Application;
using Wta.Infrastructure.Attributes;

namespace Wta.Application.Identity.Mapper;

[Service<IMapper<User, UserModel>>(ServiceLifetime.Singleton)]
public class UserMapper : DefaultMapper<User, UserModel>
{
    protected override void Config(TypeAdapterSetter<User, UserModel> toModel, TypeAdapterSetter<UserModel, User> toEntity)
    {
        toModel.Map(o => o.Roles, o => o.UserRoles.Select(o => o.RoleId));
        toEntity.Map(o => o.UserRoles, o => o.Roles.Select(r => new UserRole { RoleId = r }));
    }
}
