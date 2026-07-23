// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Text.Json;
using Data.Web.Models;
using Data.Web.Services.Foundations;

namespace Data.Web.Services.Processings;

internal sealed class DataSetProcessingService(IDataSetService dataSetService)
    : IDataSetProcessingService
{
    public Task<DataEntitySet[]> GetEntitySetsAsync(CancellationToken cancellationToken) =>
        dataSetService.GetEntitySetsAsync(cancellationToken:cancellationToken);

    public Task<DataRows> GetRowsAsync(
        string entitySet,
        int skip,
        int take,
        CancellationToken cancellationToken) =>
        dataSetService.GetRowsAsync(entitySet:entitySet, skip:skip, take:take, cancellationToken:cancellationToken);

    public Task<Dictionary<string, object>> CreateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
        dataSetService.CreateRowAsync(entitySet:entitySet, values:values, cancellationToken:cancellationToken);

    public Task<Dictionary<string, object>> UpdateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
        dataSetService.UpdateRowAsync(entitySet:entitySet, values:values, cancellationToken:cancellationToken);

    public Task DeleteRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
        dataSetService.DeleteRowAsync(entitySet:entitySet, values:values, cancellationToken:cancellationToken);
}