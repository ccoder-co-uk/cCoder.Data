// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Models;

namespace Data.Web.Exposures;

public interface IDataEntitySetManager
{
    ValueTask<DataEntitySet[]> GetEntitySetsAsync(
        CancellationToken cancellationToken);
}