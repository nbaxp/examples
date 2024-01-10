namespace Wta.Shared;

public static class IEnumerableExtensions
{
    /// <summary>
    /// IEnumerable 的 Foreach 扩展
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
    {
        values.ToList().ForEach(action);
    }
}
