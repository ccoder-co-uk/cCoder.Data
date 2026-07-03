using System.Text.Json;
using Data.Web.Models;
using Data.Web.Services.Processings;

namespace Data.Web.Services.Orchestrations;

internal sealed class DataSetOrchestrationService(IDataSetProcessingService dataSetProcessingService)
    : IDataSetOrchestrationService
{
    public Task<DataEntitySet[]> GetEntitySetsAsync(CancellationToken cancellationToken) =>
        dataSetProcessingService.GetEntitySetsAsync(cancellationToken);

    public Task<DataRows> GetRowsAsync(
        string entitySet,
        int skip,
        int take,
        CancellationToken cancellationToken) =>
            dataSetProcessingService.GetRowsAsync(entitySet, skip, take, cancellationToken);

    public Task<Dictionary<string, object>> CreateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
            dataSetProcessingService.CreateRowAsync(entitySet, values, cancellationToken);

    public Task<Dictionary<string, object>> UpdateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
            dataSetProcessingService.UpdateRowAsync(entitySet, values, cancellationToken);

    public Task DeleteRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
            dataSetProcessingService.DeleteRowAsync(entitySet, values, cancellationToken);
}
