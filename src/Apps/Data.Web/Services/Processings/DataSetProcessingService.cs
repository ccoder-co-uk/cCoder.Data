using System.Text.Json;
using Data.Web.Models;
using Data.Web.Services.Foundations;

namespace Data.Web.Services.Processings;

internal sealed class DataSetProcessingService(IDataSetService dataSetService)
    : IDataSetProcessingService
{
    public Task<DataEntitySet[]> GetEntitySetsAsync(CancellationToken cancellationToken) =>
        dataSetService.GetEntitySetsAsync(cancellationToken);

    public Task<DataRows> GetRowsAsync(
        string entitySet,
        int skip,
        int take,
        CancellationToken cancellationToken) =>
            dataSetService.GetRowsAsync(entitySet, skip, take, cancellationToken);

    public Task<Dictionary<string, object>> CreateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
            dataSetService.CreateRowAsync(entitySet, values, cancellationToken);

    public Task<Dictionary<string, object>> UpdateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
            dataSetService.UpdateRowAsync(entitySet, values, cancellationToken);

    public Task DeleteRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
            dataSetService.DeleteRowAsync(entitySet, values, cancellationToken);
}
