// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Brokers.Caching;


namespace cCoder.Data.Services.Foundations;

internal partial class MetadataTypeCacheService(IMetadataTypeCacheBroker broker)
    : IMetadataTypeCacheService
{
    public void Set(string scope, IEnumerable<string> typeSetPayloads)
    {
        ValidateScope(scope:scope);
        ValidateTypeSetPayloads(typeSetPayloads:typeSetPayloads);

        broker.Set(scope:scope, typeSetPayloads:typeSetPayloads.ToArray());
    }

    public string[] Get(string scope)
    {
        ValidateScope(scope:scope);

        return broker.Get(scope:scope);
    }

    public string[] GetAll() =>
        broker.GetAll();

    public bool Contains(string scope)
    {
        ValidateScope(scope:scope);

        return broker.Contains(scope:scope);
    }

    public void Clear(string scope)
    {
        ValidateScope(scope:scope);

        broker.Clear(scope:scope);
    }
}