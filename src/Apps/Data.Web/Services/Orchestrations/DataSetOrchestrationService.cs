// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Text.Json;
using Data.Web.Models;
using Data.Web.Services.Processings;

namespace Data.Web.Services.Orchestrations;

internal sealed class DataSetOrchestrationService(IDataSetProcessingService dataSetProcessingService)
    : IDataSetOrchestrationService
{
    public Task<DataEntitySet[]> GetEntitySetsAsync(CancellationToken cancellationToken) =>
        dataSetProcessingService.GetEntitySetsAsync(cancellationToken:cancellationToken);

    public Task<DataRows> GetRowsAsync(
        string entitySet,
        int skip,
        int take,
        CancellationToken cancellationToken) =>
        dataSetProcessingService.GetRowsAsync(entitySet:entitySet, skip:skip, take:take, cancellationToken:cancellationToken);

    public Task<Dictionary<string, object>> CreateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
        dataSetProcessingService.CreateRowAsync(entitySet:entitySet, values:values, cancellationToken:cancellationToken);

    public Task<Dictionary<string, object>> UpdateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
        dataSetProcessingService.UpdateRowAsync(entitySet:entitySet, values:values, cancellationToken:cancellationToken);

    public Task DeleteRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
        dataSetProcessingService.DeleteRowAsync(entitySet:entitySet, values:values, cancellationToken:cancellationToken);
}