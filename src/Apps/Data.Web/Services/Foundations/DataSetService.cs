using System.Security;
using System.Text.Json;
using Data.Web.Brokers;
using Data.Web.Models;

namespace Data.Web.Services.Foundations;

internal sealed class DataSetService(IDataSetBroker dataSetBroker)
    : IDataSetService
{
    public Task<DataEntitySet[]> GetEntitySetsAsync(CancellationToken cancellationToken)
    {
        EnsureAuthenticated();

        return dataSetBroker.SelectEntitySetsAsync(cancellationToken);
    }

    public Task<DataRows> GetRowsAsync(
        string entitySet,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        EnsureAuthenticated();

        return dataSetBroker.SelectRowsAsync(
            entitySet,
            Math.Max(skip, 0),
            Math.Clamp(take, 1, 500),
            cancellationToken);
    }

    public Task<Dictionary<string, object>> CreateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken)
    {
        EnsureAuthenticated();

        return dataSetBroker.InsertRowAsync(entitySet, values, cancellationToken);
    }

    public Task<Dictionary<string, object>> UpdateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken)
    {
        EnsureAuthenticated();

        return dataSetBroker.UpdateRowAsync(entitySet, values, cancellationToken);
    }

    public Task DeleteRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken)
    {
        EnsureAuthenticated();

        return dataSetBroker.DeleteRowAsync(entitySet, values, cancellationToken);
    }

    private void EnsureAuthenticated()
    {
        string ssoUserId = dataSetBroker.GetCurrentSsoUserId();

        if (string.IsNullOrWhiteSpace(ssoUserId) || ssoUserId == "Guest")
            throw new SecurityException("Authentication is required.");
    }
}
