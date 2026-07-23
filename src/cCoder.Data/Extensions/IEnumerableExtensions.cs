// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Extensions;

public static class IEnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
    {
        if (source is null)
            return;

        int index = 0;
        foreach (T item in source)
            action(arg1:item, arg2:index++);
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        if (source is null)
            return;

        foreach (T item in source)
            action(obj:item);
    }
}