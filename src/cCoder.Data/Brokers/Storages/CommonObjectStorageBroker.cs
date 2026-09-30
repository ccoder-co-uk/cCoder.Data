// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Linq;
using cCoder.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace cCoder.Data.Brokers.Storages;

internal sealed class CommonObjectStorageBroker(
    ICoreContextFactory coreContextFactory)
    : ICommonObjectStorageBroker
{
    public IQueryable<CommonObject> GetAllCommonObjects()
    {
        CoreDataContext coreDataContext =
            coreContextFactory.CreateCoreContext();

        return coreDataContext.CommonObjects;
    }

    public IQueryable<CommonObject> GetAllCommonObjectsIgnoringFilters()
    {
        CoreDataContext coreDataContext =
            coreContextFactory.CreateCoreContext();

        return coreDataContext.CommonObjects.IgnoreQueryFilters();
    }

    public CommonObject[] GetCommonObjectsSnapshot()
    {
        using CoreDataContext coreDataContext =
            coreContextFactory.CreateCoreContext();

        return coreDataContext.CommonObjects
            .IgnoreQueryFilters()
            .AsNoTracking()
            .ToArray();
    }

    public async ValueTask<CommonObject> AddCommonObjectAsync(
        CommonObject newCommonObject)
    {
        using CoreDataContext coreDataContext =
            coreContextFactory.CreateCoreContext();

        CommonObject result = (await coreDataContext.CommonObjects
            .AddAsync(entity: newCommonObject))
            .Entity;

        await coreDataContext.SaveChangesAsync();
        return result;
    }

    public async ValueTask<CommonObject> UpdateCommonObjectAsync(
        CommonObject updatedCommonObject)
    {
        using CoreDataContext coreDataContext =
            coreContextFactory.CreateCoreContext();

        CommonObject result = coreDataContext.CommonObjects
            .Update(entity: updatedCommonObject)
            .Entity;

        await coreDataContext.SaveChangesAsync();
        return result;
    }

    public async ValueTask<int> DeleteCommonObjectAsync(
        CommonObject deletedCommonObject)
    {
        using CoreDataContext coreDataContext =
            coreContextFactory.CreateCoreContext();

        coreDataContext.CommonObjects.Remove(
            entity: deletedCommonObject);

        return await coreDataContext.SaveChangesAsync();
    }

}