// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models;

namespace cCoder.Data.Services.Foundations;

internal interface ICommonObjectCacheService
{
    CommonObject[] GetCommonObjects();

    void SetCommonObjects(CommonObject[] commonObjects);
}