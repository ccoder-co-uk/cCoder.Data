// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Linq;
using cCoder.Data.Brokers.Storages;
using cCoder.Data.Models;

namespace cCoder.Data.Services.Foundations;

internal sealed partial class CommonObjectStorageService(
    ICommonObjectStorageBroker commonObjectStorageBroker)
    : ICommonObjectStorageService
{
    public IQueryable<CommonObject> GetCommonObjects(bool ignoreFilters) =>
        TryCatch(operation: () =>
        {
            ValidateCommonObjectsOnGet(inputs: [ignoreFilters]);

            return ignoreFilters
                ? commonObjectStorageBroker
                    .GetAllCommonObjectsIgnoringFilters()
                : commonObjectStorageBroker.GetAllCommonObjects();
        });

    public CommonObject[] GetCommonObjectSnapshot() =>
        TryCatch(operation: () =>
        {
            return commonObjectStorageBroker.GetCommonObjectSnapshot();
        });

    public ValueTask<CommonObject> AddCommonObjectAsync(
        CommonObject newCommonObject) =>
        TryCatch<CommonObject>(operation: async () =>
        {
            ValidateCommonObjectOnAdd(inputs: [newCommonObject]);

            return await commonObjectStorageBroker.AddCommonObjectAsync(
                newCommonObject: newCommonObject);
        }, isValueTask: true);

    public ValueTask<CommonObject> UpdateCommonObjectAsync(
        CommonObject updatedCommonObject) =>
        TryCatch<CommonObject>(operation: async () =>
        {
            ValidateCommonObjectOnUpdate(inputs: [updatedCommonObject]);

            CommonObject storageCommonObject =
                CreateStorageCommonObject(
                    commonObject: updatedCommonObject);

            return await commonObjectStorageBroker.UpdateCommonObjectAsync(
                updatedCommonObject: storageCommonObject);
        }, isValueTask: true);

    public ValueTask<int> DeleteCommonObjectAsync(
        CommonObject deletedCommonObject) =>
        TryCatch<int>(operation: async () =>
        {
            ValidateCommonObjectOnDelete(inputs: [deletedCommonObject]);

            return await commonObjectStorageBroker.DeleteCommonObjectAsync(
                deletedCommonObject: deletedCommonObject);
        }, isValueTask: true);

    private static CommonObject CreateStorageCommonObject(
        CommonObject commonObject) =>
        new CommonObject
        {
            Id = commonObject.Id,
            Name = commonObject.Name,
            Description = commonObject.Description,
            LastUpdated = commonObject.LastUpdated,
            LastUpdatedBy = commonObject.LastUpdatedBy,
            CreatedOn = commonObject.CreatedOn,
            CreatedBy = commonObject.CreatedBy,
            Version = commonObject.Version,
            Key = commonObject.Key,
            Type = commonObject.Type,
            Json = commonObject.Json,
            Culture = commonObject.Culture
        };
}