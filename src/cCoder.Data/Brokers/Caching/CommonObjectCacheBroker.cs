// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models;

namespace cCoder.Data.Brokers.Caching;

internal sealed class CommonObjectCacheBroker : ICommonObjectCacheBroker
{
    private readonly object syncRoot = new();
    private CommonObject[] commonObjects;

    public CommonObject[] GetCommonObjects()
    {
        lock (syncRoot)
        {
            return commonObjects;
        }
    }

    public void SetCommonObjects(CommonObject[] commonObjects)
    {
        lock (syncRoot)
        {
            this.commonObjects = commonObjects;
        }
    }
}