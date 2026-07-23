// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Exposures;

public interface IMetadataTypeCache
{
    void Set(string scope, IEnumerable<string> typeSetPayloads);
    string[] Get(string scope);
    string[] GetAll();
    bool Contains(string scope);
    void Clear(string scope);
}