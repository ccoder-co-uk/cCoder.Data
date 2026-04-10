namespace cCoder.Data.Extensions;

public static class IEnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
    {
        if (source is null)
            return;

        int index = 0;
        foreach (T item in source)
            action(item, index++);
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        if (source is null)
            return;

        foreach (T item in source)
            action(item);
    }
}
