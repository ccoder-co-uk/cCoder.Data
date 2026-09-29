// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models;

namespace cCoder.Data.Exposures;

public interface ICommonObjectCacheManager
{
    IQueryable<CommonObject> Get(
        bool fromCache,
        bool ignoreFilters = false);

    ValueTask<CommonObject> AddAsync(CommonObject newCommonObject);

    ValueTask<CommonObject> UpdateAsync(CommonObject updatedCommonObject);

    ValueTask<int> DeleteAsync(CommonObject deletedCommonObject);

    ValueTask DeleteAllAsync(IEnumerable<CommonObject> deletedCommonObjects);

    void Refresh();
}