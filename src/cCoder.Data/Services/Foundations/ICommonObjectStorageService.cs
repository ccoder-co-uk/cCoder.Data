// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Linq;
using cCoder.Data.Models;

namespace cCoder.Data.Services.Foundations;

internal interface ICommonObjectStorageService
{
    IQueryable<CommonObject> GetCommonObjects(bool ignoreFilters);

    CommonObject[] GetCommonObjectSnapshot();

    ValueTask<CommonObject> AddCommonObjectAsync(CommonObject newCommonObject);

    ValueTask<CommonObject> UpdateCommonObjectAsync(CommonObject updatedCommonObject);

    ValueTask<int> DeleteCommonObjectAsync(CommonObject deletedCommonObject);
}