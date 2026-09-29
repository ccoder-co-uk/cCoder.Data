// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using Data.Web.Models;

namespace Data.Web.Services.Foundations;

public interface IDataEntitySetService
{
    ValueTask<DataEntitySet[]> GetEntitySetsAsync(
        CancellationToken cancellationToken);
}