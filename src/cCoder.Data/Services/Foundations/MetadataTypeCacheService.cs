using cCoder.Data.Brokers.Caching;


namespace cCoder.Data.Services.Foundations;

internal partial class MetadataTypeCacheService(IMetadataTypeCacheBroker broker)
    : IMetadataTypeCacheService
{
    public void Set(string scope, IEnumerable<string> typeSetPayloads)
    {
        ValidateScope(scope);
        ValidateTypeSetPayloads(typeSetPayloads);

        broker.Set(scope, typeSetPayloads.ToArray());
    }

    public string[] Get(string scope)
    {
        ValidateScope(scope);

        return broker.Get(scope);
    }

    public string[] GetAll() => broker.GetAll();

    public bool Contains(string scope)
    {
        ValidateScope(scope);

        return broker.Contains(scope);
    }

    public void Clear(string scope)
    {
        ValidateScope(scope);

        broker.Clear(scope);
    }
}


