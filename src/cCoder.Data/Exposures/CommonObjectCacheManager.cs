// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models;
using cCoder.Data.Services.Orchestrations;

namespace cCoder.Data.Exposures;

internal sealed class CommonObjectCacheManager(
    ICommonObjectCacheOrchestrationService orchestrationService)
    : ICommonObjectCacheManager
{
    public IQueryable<CommonObject> Get(
        bool fromCache,
        bool ignoreFilters = false) =>
        orchestrationService.GetCommonObjects(
            fromCache: fromCache,
            ignoreFilters: ignoreFilters);

    public ValueTask<CommonObject> AddAsync(
        CommonObject newCommonObject) =>
        orchestrationService.AddCommonObjectAsync(
            newCommonObject: newCommonObject);

    public ValueTask<CommonObject> UpdateAsync(
        CommonObject updatedCommonObject) =>
        orchestrationService.UpdateCommonObjectAsync(
            updatedCommonObject: updatedCommonObject);

    public ValueTask<int> DeleteAsync(
        CommonObject deletedCommonObject) =>
        orchestrationService.DeleteCommonObjectAsync(
            deletedCommonObject: deletedCommonObject);

    public ValueTask DeleteAllAsync(
        IEnumerable<CommonObject> deletedCommonObjects) =>
        orchestrationService.DeleteAllCommonObjectsAsync(
            deletedCommonObjects: deletedCommonObjects);

    public void Refresh() =>
        orchestrationService.RefreshCommonObjects();
}