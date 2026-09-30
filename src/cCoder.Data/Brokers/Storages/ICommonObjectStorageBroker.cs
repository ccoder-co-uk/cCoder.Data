// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Linq;
using cCoder.Data.Models;

namespace cCoder.Data.Brokers.Storages;

internal interface ICommonObjectStorageBroker
{
    IQueryable<CommonObject> GetAllCommonObjects();

    IQueryable<CommonObject> GetAllCommonObjectsIgnoringFilters();

    CommonObject[] GetCommonObjectsSnapshot();

    ValueTask<CommonObject> AddCommonObjectAsync(CommonObject newCommonObject);

    ValueTask<CommonObject> UpdateCommonObjectAsync(CommonObject updatedCommonObject);

    ValueTask<int> DeleteCommonObjectAsync(CommonObject deletedCommonObject);

}