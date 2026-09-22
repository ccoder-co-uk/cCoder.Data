// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

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
        Dictionary<string, object> newValues,
        CancellationToken cancellationToken);

    ValueTask<Dictionary<string, object>> UpdateRowAsync(
        string entitySet,
        Dictionary<string, object> updatedValues,
        CancellationToken cancellationToken);

    ValueTask DeleteRowAsync(
        string entitySet,
        Dictionary<string, object> deletedValues,
        CancellationToken cancellationToken);
}