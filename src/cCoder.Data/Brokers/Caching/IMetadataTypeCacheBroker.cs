// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Brokers.Caching;

internal interface IMetadataTypeCacheBroker
{
    void Set(string scope, string[] typeSetPayloads);
    string[] Get(string scope);
    string[] GetAll();
    bool Contains(string scope);
    void Clear(string scope);
}