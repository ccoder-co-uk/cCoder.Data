using System.Collections.Concurrent;

namespace cCoder.Data.Brokers.Caching;

internal sealed class MetadataTypeCacheBroker : IMetadataTypeCacheBroker
{
    private readonly ConcurrentDictionary<string, string[]> cache = new(
        StringComparer.OrdinalIgnoreCase
    );

    public void Set(string scope, string[] typeSetPayloads) => cache[scope] = typeSetPayloads;

    public string[] Get(string scope) =>
        cache.TryGetValue(scope, out string[] typeSetPayloads) ? typeSetPayloads : [];

    public string[] GetAll() => cache.Values.SelectMany(value => value).ToArray();

    public bool Contains(string scope) => cache.ContainsKey(scope);

    public void Clear(string scope) => cache.TryRemove(scope, out _);
}


