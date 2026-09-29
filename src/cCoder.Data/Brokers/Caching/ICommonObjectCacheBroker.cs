// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models;

namespace cCoder.Data.Brokers.Caching;

internal interface ICommonObjectCacheBroker
{
    CommonObject[] GetCommonObjects();

    void SetCommonObjects(CommonObject[] commonObjects);
}