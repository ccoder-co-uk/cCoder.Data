using cCoder.Data.Services.Foundations;


namespace cCoder.Data.Exposures;

public sealed class MetadataTypeCache(IMetadataTypeCacheService service) : IMetadataTypeCache
{
    public void Set(string scope, IEnumerable<string> typeSetPayloads) =>
        service.Set(scope, typeSetPayloads);

    public string[] Get(string scope) => service.Get(scope);

    public string[] GetAll() => service.GetAll();

    public bool Contains(string scope) => service.Contains(scope);

    public void Clear(string scope) => service.Clear(scope);
}


