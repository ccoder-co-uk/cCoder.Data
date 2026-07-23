// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Text.Json;
using Data.Web.Models;

namespace Data.Web.Dependencies;

internal interface IDataSetDependency
{
    string GetCurrentSsoUserId();

    Task<DataEntitySet[]> SelectEntitySetsAsync(CancellationToken cancellationToken);

    Task<DataRows> SelectRowsAsync(
        string entitySet,
        int skip,
        int take,
        CancellationToken cancellationToken);

    Task<Dictionary<string, object>> InsertRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken);

    Task<Dictionary<string, object>> UpdateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken);

    Task DeleteRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken);
}