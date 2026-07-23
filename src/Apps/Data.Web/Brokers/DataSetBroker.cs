// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Text.Json;
using Data.Web.Dependencies;
using Data.Web.Models;

namespace Data.Web.Brokers;

internal sealed class DataSetBroker(IDataSetDependency dataSetDependency)
    : IDataSetBroker
{
    public string GetCurrentSsoUserId() =>
        dataSetDependency.GetCurrentSsoUserId();

    public Task<DataEntitySet[]> SelectEntitySetsAsync(CancellationToken cancellationToken) =>
        dataSetDependency.SelectEntitySetsAsync(cancellationToken: cancellationToken);

    public Task<DataRows> SelectRowsAsync(
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
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
        dataSetDependency.InsertRowAsync(
            entitySet: entitySet,
            values: values,
            cancellationToken: cancellationToken);

    public Task<Dictionary<string, object>> UpdateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
        dataSetDependency.UpdateRowAsync(
            entitySet: entitySet,
            values: values,
            cancellationToken: cancellationToken);

    public Task DeleteRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
        dataSetDependency.DeleteRowAsync(
            entitySet: entitySet,
            values: values,
            cancellationToken: cancellationToken);
}