// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using cCoder.Data.Models;

namespace cCoder.Data.Services.Orchestrations;

internal interface ICommonObjectCacheOrchestrationService
{
    IQueryable<CommonObject> GetCommonObjects(
        bool fromCache,
        bool ignoreFilters = false);

    ValueTask<CommonObject> AddCommonObjectAsync(CommonObject newCommonObject);

    ValueTask<CommonObject> UpdateCommonObjectAsync(CommonObject updatedCommonObject);

    ValueTask<int> DeleteCommonObjectAsync(CommonObject deletedCommonObject);

    ValueTask DeleteAllCommonObjectsAsync(IEnumerable<CommonObject> deletedCommonObjects);

    void RefreshCommonObjects();
}