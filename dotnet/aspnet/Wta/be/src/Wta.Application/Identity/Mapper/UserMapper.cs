using Mapster;
using Wta.Application.Identity.Domain;
using Wta.Application.Identity.Models;
using Wta.Infrastructure.Application;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Interfaces;

namespace Wta.Application.Identity.Mapper;

[Service<IMapper<User, UserModel>>(ServiceLifetime.Singleton)]
public class UserMapper(IEncryptionService passwordHasher) : DefaultMapper<User, UserModel>
{
    protected override void Config(TypeAdapterSetter<User, UserModel> toModel, TypeAdapterSetter<UserModel, User> toEntity)
    {
        toModel.Map(o => o.Roles, o => o.UserRoles.Select(o => o.RoleId));
        toEntity.Map(o => o.UserRoles, o => o.Roles.Select(r => new UserRole { RoleId = r }));
        toEntity.Map(o => o.NormalizedUserName, o => o.UserName!.ToUpperInvariant());
    }

    public override User FromModel(User entity, UserModel model)
    {
        base.FromModel(entity, model);
        if (string.IsNullOrEmpty(entity.SecurityStamp))
        {
            entity.SecurityStamp = passwordHasher.CreateSalt();
        }
        if (!string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(entity.SecurityStamp))
        {
            entity.PasswordHash = passwordHasher.HashPassword(model.Password, entity.SecurityStamp);
        }
        return entity;
    }
}
