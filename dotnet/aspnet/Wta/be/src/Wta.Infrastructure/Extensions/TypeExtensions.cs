using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wta.Infrastructure.Extensions;

public static class TypeExtensions
{
    /// <summary>
    /// 获取全部基类
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type[] GetBaseClasses(this Type type)
    {
        var classes = new List<Type>();
        var current = type;
        while (current.BaseType != null && current.BaseType.IsClass && current.BaseType != typeof(object))
        {
            classes.Add(current.BaseType);
            current = current.BaseType;
        }
        return classes.ToArray();
    }

    /// <summary>
    /// 是否可空值类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsNullableType(this Type type)
    {
        return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    /// <summary>
    /// 原始类型（如果是可空值类型，返回Nullable的泛型参数类型）
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type GetUnderlyingType(this Type type)
    {
        return type.IsNullableType() ? Nullable.GetUnderlyingType(type)! : type;
    }

    /// <summary>
    /// 是否具有特性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    public static bool HasAttribute<T>(this Type type, bool inherit = true) where T : Attribute
    {
        return type.GetCustomAttributes<T>(inherit).Any();
    }

    /// <summary>
    /// 返回DisplayAttribute.Name
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetDisplayName(this Type type)
    {
        var scope = WtaApplication.Application.Services.CreateScope();
        var localizer = scope?.ServiceProvider.GetService<IStringLocalizer>();
        var key = type.GetCustomAttribute<DisplayAttribute>()?.Name ?? type.Name;
        return localizer?.GetString(key, type.FullName!) ?? key;
    }

    public static void InvokeMethod(this Type type, string name, object? instance, Type[]? typeArguments, Type[]? parameterTypes, params object[] parameters)
    {
        parameterTypes ??= parameters.Select(o => o.GetType()).ToArray();
        var method = type.GetMethods()
            .FirstOrDefault(o => o.Name == name && o.GetParameters().Select(o => o.ParameterType).SequenceEqual(parameterTypes));
        if (method != null && typeArguments != null)
        {
            method = method.MakeGenericMethod(typeArguments);
        }
        method?.MakeGenericMethod().Invoke(instance, parameters);
    }

    public static void InvokeExtensionMethod(this Type type, string name, Type[]? typeArguments, Type[] parameterTypes, params object[] parameters)
    {
        parameterTypes ??= parameters.Select(o => o.GetType()).Skip(1).ToArray();
        var method = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(o => o.IsDefined(typeof(ExtensionAttribute)) && o.Name == name && o.GetParameters().Select(o => o.ParameterType).Skip(1).SequenceEqual(parameterTypes));
        if (method != null && typeArguments != null)
        {
            method = method.MakeGenericMethod(typeArguments);
        }
        method?.Invoke(null, parameters);
    }

    public static object GetMetadataForType(this Type modelType)
    {
        using var scope = WtaApplication.Application.Services.CreateScope();
        var meta = scope.ServiceProvider.GetRequiredService<IModelMetadataProvider>().GetMetadataForType(modelType);
        return meta.GetSchema(scope.ServiceProvider);
    }
}
