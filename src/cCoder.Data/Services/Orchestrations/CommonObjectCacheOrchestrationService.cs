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

            _ = RefreshCommonObjectsCore();
            return result;
        }, isValueTask: true);

    public ValueTask<CommonObject> UpdateCommonObjectAsync(
        CommonObject updatedCommonObject) =>
        TryCatch<CommonObject>(operation: async () =>
        {
            ValidateCommonObjectOnUpdate(inputs: [updatedCommonObject]);

            CommonObject result =
                await commonObjectStorageService.UpdateCommonObjectAsync(
                    updatedCommonObject: updatedCommonObject);

            _ = RefreshCommonObjectsCore();
            return result;
        }, isValueTask: true);

    public ValueTask<int> DeleteCommonObjectAsync(
        CommonObject deletedCommonObject) =>
        TryCatch<int>(operation: async () =>
        {
            ValidateCommonObjectOnDelete(inputs: [deletedCommonObject]);

            int result =
                await commonObjectStorageService.DeleteCommonObjectAsync(
                    deletedCommonObject: deletedCommonObject);

            _ = RefreshCommonObjectsCore();
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

            _ = RefreshCommonObjectsCore();
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
}