// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models;

namespace cCoder.Data.Brokers.Storages;

internal interface ICommonObjectStorageBroker
{
    IQueryable<CommonObject> GetAllCommonObjects();

    IQueryable<CommonObject> GetAllCommonObjectsIgnoringFilters();

    CommonObject[] GetCommonObjectSnapshot();

    ValueTask<CommonObject> AddCommonObjectAsync(CommonObject newCommonObject);

    ValueTask<CommonObject> UpdateCommonObjectAsync(CommonObject updatedCommonObject);

    ValueTask<int> DeleteCommonObjectAsync(CommonObject deletedCommonObject);

}