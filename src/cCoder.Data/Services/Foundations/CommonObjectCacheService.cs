// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Brokers.Caching;
using cCoder.Data.Models;

namespace cCoder.Data.Services.Foundations;

internal sealed partial class CommonObjectCacheService(
    ICommonObjectCacheBroker commonObjectCacheBroker)
    : ICommonObjectCacheService
{
    public CommonObject[] GetCommonObjects() =>
        TryCatch(operation: () =>
        {
            return commonObjectCacheBroker.GetCommonObjects();
        });

    public void SetCommonObjects(CommonObject[] commonObjects) =>
        TryCatch(operation: () =>
        {
            ValidateCommonObjectsOnSet(inputs: [commonObjects]);

            commonObjectCacheBroker.SetCommonObjects(
                commonObjects: commonObjects);
        });
}