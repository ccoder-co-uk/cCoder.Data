// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Models;

namespace Data.Web.Brokers;

internal interface IDataSetBroker
{
    string GetCurrentSsoUserId();

    Task<(
        string Name,
        string DisplayName,
        string ClrType,
        string Table,
        string[] KeyProperties,
        (
            string Name,
            string Type,
            bool IsKey,
            bool IsNullable,
            bool CanCreate,
            bool CanUpdate,
            bool IsLongText)[] Properties)[]> SelectEntitySetsAsync(
        CancellationToken cancellationToken);

    Task<(string EntitySet, Dictionary<string, object>[] Rows)> SelectRowsAsync(
        string entitySet,
        int skip,
        int take,
        CancellationToken cancellationToken);

    Task<Dictionary<string, object>> InsertRowAsync(
        string entitySet,
        Dictionary<string, object> values,
        CancellationToken cancellationToken);

    Task<Dictionary<string, object>> UpdateRowAsync(
        string entitySet,
        Dictionary<string, object> values,
        CancellationToken cancellationToken);

    Task DeleteRowAsync(
        string entitySet,
        Dictionary<string, object> values,
        CancellationToken cancellationToken);
}