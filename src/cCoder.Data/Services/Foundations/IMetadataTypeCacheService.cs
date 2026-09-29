// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Collections.Generic;

namespace cCoder.Data.Services.Foundations;

internal interface IMetadataTypeCacheService
{
    void Set(string scope, IEnumerable<string> typeSetPayloads);
    string[] Get(string scope);
    string[] GetAll();
    bool Contains(string scope);
    void Clear(string scope);
}