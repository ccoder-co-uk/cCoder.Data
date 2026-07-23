// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Models;

namespace Data.Web.Services.Foundations;

public interface IDataEntitySetService
{
    ValueTask<DataEntitySet[]> GetEntitySetsAsync(
        CancellationToken cancellationToken);
}