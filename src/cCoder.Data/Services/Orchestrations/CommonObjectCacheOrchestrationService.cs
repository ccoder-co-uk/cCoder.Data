// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using cCoder.Data.Models;
using cCoder.Data.Services.Foundations;

namespace cCoder.Data.Services.Orchestrations;

internal sealed partial class CommonObjectCacheOrchestrationService(
    ICommonObjectStorageService commonObjectStorageService,
    ICommonObjectCacheService commonObjectCacheService)
    : ICommonObjectCacheOrchestrationService
{
    public IQueryable<CommonObject> GetCommonObjects(
        bool fromCache,
        bool ignoreFilters = false) =>
        TryCatch(operation: () =>
        {
            ValidateCommonObjectsOnGet(
                inputs: [fromCache, ignoreFilters]);

            if (!fromCache)
            {
                return commonObjectStorageService.GetCommonObjects(
                    ignoreFilters: ignoreFilters);
            }

            CommonObject[] commonObjects =
                commonObjectCacheService.GetCommonObjects();

            if (commonObjects is not null)
            {
                return commonObjects.AsQueryable();
            }

            return RefreshCommonObjectsCore()
                .AsQueryable();
        });

    public ValueTask<CommonObject> AddCommonObjectAsync(
        CommonObject newCommonObject) =>
        TryCatch<CommonObject>(operation: async () =>
        {
            ValidateCommonObjectOnAdd(inputs: [newCommonObject]);

            CommonObject result =
                await commonObjectStorageService.AddCommonObjectAsync(
                    newCommonObject: newCommonObject);

            AddCommonObjectsToCache(newCommonObjects: [result]);

            return result;
        }, isValueTask: true);

    public ValueTask<CommonObject[]> AddAllCommonObjectsAsync(
        CommonObject[] newCommonObjects) =>
        TryCatch<CommonObject[]>(operation: async () =>
        {
            ValidateAllCommonObjectsOnAdd(inputs: [newCommonObjects]);

            CommonObject[] results =
                await commonObjectStorageService.AddAllCommonObjectsAsync(
                    newCommonObjects: newCommonObjects);

            AddCommonObjectsToCache(newCommonObjects: results);

            return results;
        }, isValueTask: true);

    public ValueTask<CommonObject> UpdateCommonObjectAsync(
        CommonObject updatedCommonObject) =>
        TryCatch<CommonObject>(operation: async () =>
        {
            ValidateCommonObjectOnUpdate(inputs: [updatedCommonObject]);

            CommonObject result =
                await commonObjectStorageService.UpdateCommonObjectAsync(
                    updatedCommonObject: updatedCommonObject);

            UpdateCommonObjectInCache(updatedCommonObject: result);

            return result;
        }, isValueTask: true);

    public void ApplyCommonObjects(CommonObject[] changedCommonObjects) =>
        TryCatch(operation: () =>
        {
            ValidateCommonObjectsOnApply(inputs: [changedCommonObjects]);

            CommonObject[] current = commonObjectCacheService.GetCommonObjects();

            if (current is not null)
            {
                Dictionary<int, CommonObject> commonObjects = current
                    .ToDictionary(keySelector: item => item.Id);

                foreach (CommonObject changedCommonObject in changedCommonObjects)
                {
                    commonObjects[changedCommonObject.Id] = changedCommonObject;
                }

                commonObjectCacheService.SetCommonObjects(
                    commonObjects: commonObjects.Values.ToArray());
            }
        });

    public ValueTask<int> DeleteCommonObjectAsync(
        CommonObject deletedCommonObject) =>
        TryCatch<int>(operation: async () =>
        {
            ValidateCommonObjectOnDelete(inputs: [deletedCommonObject]);

            int result =
                await commonObjectStorageService.DeleteCommonObjectAsync(
                    deletedCommonObject: deletedCommonObject);

            DeleteCommonObjectsFromCache(
                deletedCommonObjects: [deletedCommonObject]);

            return result;
        }, isValueTask: true);

    public ValueTask DeleteAllCommonObjectsAsync(
        IEnumerable<CommonObject> deletedCommonObjects) =>
        TryCatch(operation: async () =>
        {
            ValidateAllCommonObjectsOnDelete(inputs: [deletedCommonObjects]);

            foreach (CommonObject commonObject in deletedCommonObjects)
            {
                await commonObjectStorageService.DeleteCommonObjectAsync(
                    deletedCommonObject: commonObject);
            }

            DeleteCommonObjectsFromCache(
                deletedCommonObjects: deletedCommonObjects.ToArray());
        }, isValueTask: true);

    public void RefreshCommonObjects() =>
        TryCatch(operation: () =>
        {
            ValidateCommonObjectsOnRefresh();
            _ = RefreshCommonObjectsCore();
        });

    private CommonObject[] RefreshCommonObjectsCore()
    {
        CommonObject[] commonObjects =
            commonObjectStorageService.GetCommonObjectsSnapshot();

        commonObjectCacheService.SetCommonObjects(
            commonObjects: commonObjects);

        return commonObjects;
    }

    private void AddCommonObjectsToCache(CommonObject[] newCommonObjects)
    {
        CommonObject[] commonObjects = commonObjectCacheService.GetCommonObjects();

        if (commonObjects is not null)
        {
            commonObjectCacheService.SetCommonObjects(
                commonObjects: [.. commonObjects, .. newCommonObjects]);
        }
    }

    private void UpdateCommonObjectInCache(CommonObject updatedCommonObject)
    {
        CommonObject[] commonObjects = commonObjectCacheService.GetCommonObjects();

        if (commonObjects is not null)
        {
            commonObjectCacheService.SetCommonObjects(
                commonObjects: commonObjects
                    .Select(selector: item => item.Id == updatedCommonObject.Id
                        ? updatedCommonObject
                        : item)
                    .ToArray());
        }
    }

    private void DeleteCommonObjectsFromCache(
        CommonObject[] deletedCommonObjects)
    {
        CommonObject[] commonObjects = commonObjectCacheService.GetCommonObjects();

        if (commonObjects is not null)
        {
            HashSet<int> deletedIds = deletedCommonObjects
                .Select(selector: item => item.Id)
                .ToHashSet();

            commonObjectCacheService.SetCommonObjects(
                commonObjects: commonObjects
                    .Where(predicate: item => !deletedIds.Contains(item: item.Id))
                    .ToArray());
        }
    }
}