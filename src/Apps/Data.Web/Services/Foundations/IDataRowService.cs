// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Text.Json;
using Data.Web.Models;

namespace Data.Web.Services.Foundations;

public interface IDataRowService
{
    ValueTask<DataRows> GetRowsAsync(
        string entitySet,
        int skip,
        int take,
        CancellationToken cancellationToken);

    ValueTask<Dictionary<string, object>> AddRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> newValues,
        CancellationToken cancellationToken);

    ValueTask<Dictionary<string, object>> UpdateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> updatedValues,
        CancellationToken cancellationToken);

    ValueTask DeleteRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> deletedValues,
        CancellationToken cancellationToken);
}