// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Collections.Concurrent;

namespace cCoder.Data.Brokers.Caching;

internal sealed class MetadataTypeCacheBroker : IMetadataTypeCacheBroker
{
    private readonly ConcurrentDictionary<string, string[]> cache = new(
        StringComparer.OrdinalIgnoreCase
    );

    public void Set(string scope, string[] typeSetPayloads) =>
        cache[scope] = typeSetPayloads;

    public string[] Get(string scope) =>
        cache.GetValueOrDefault(
            key: scope,
            defaultValue: []);

    public string[] GetAll() =>
        cache.Values
            .SelectMany(selector: value => value)
            .ToArray();

    public bool Contains(string scope) =>
        cache.ContainsKey(key:scope);

    public void Clear(string scope) =>
        cache.TryRemove(key:scope, value:out _);
}