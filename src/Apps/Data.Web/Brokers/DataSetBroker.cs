// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Dependencies;

namespace Data.Web.Brokers;

internal sealed class DataSetBroker(DataSetDependency dataSetDependency)
    : IDataSetBroker
{
    public string GetCurrentSsoUserId() =>
        dataSetDependency.GetCurrentSsoUserId();

    public Task<(
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
        CancellationToken cancellationToken) =>
        dataSetDependency.SelectEntitySetsAsync(
            cancellationToken: cancellationToken);

    public Task<(string EntitySet, Dictionary<string, object>[] Rows)> SelectRowsAsync(
        string entitySet,
        int skip,
        int take,
        CancellationToken cancellationToken) =>
        dataSetDependency.SelectRowsAsync(
            entitySet: entitySet,
            skip: skip,
            take: take,
            cancellationToken: cancellationToken);

    public Task<Dictionary<string, object>> InsertRowAsync(
        string entitySet,
        Dictionary<string, object> values,
        CancellationToken cancellationToken) =>
        dataSetDependency.InsertRowAsync(
            entitySet: entitySet,
            values: values,
            cancellationToken: cancellationToken);

    public Task<Dictionary<string, object>> UpdateRowAsync(
        string entitySet,
        Dictionary<string, object> values,
        CancellationToken cancellationToken) =>
        dataSetDependency.UpdateRowAsync(
            entitySet: entitySet,
            values: values,
            cancellationToken: cancellationToken);

    public Task DeleteRowAsync(
        string entitySet,
        Dictionary<string, object> values,
        CancellationToken cancellationToken) =>
        dataSetDependency.DeleteRowAsync(
            entitySet: entitySet,
            values: values,
            cancellationToken: cancellationToken);
}