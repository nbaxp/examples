using Mapster;

namespace Wta.Infrastructure.Extensions;

public static class ObjectExtensions
{
    static ObjectExtensions()
    {
        TypeAdapterConfig.GlobalSettings.Default.IgnoreMember((member, side) =>
        {
            return side == MemberSide.Destination && member.Name == "Id" && member.Type == typeof(Guid);
        });
    }

    public static TTarget UpdateFrom<TTarget, TSource>(this TTarget target, TSource source)
    {
        source.Adapt(target);
        return target;
    }

    public static TTarget MapTo<TTarget>(this object source) where TTarget : class
    {
        if (source is IQueryable queryable && typeof(TTarget) is List<TTarget>)
        {
            return (queryable.ProjectToType<TTarget>().ToList() as TTarget)!;
        }
        var to = source.Adapt<TTarget>();
        return to;
    }
}
