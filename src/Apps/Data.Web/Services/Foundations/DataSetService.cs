// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Security;
using System.Text.Json;
using Data.Web.Dependencies;
using Data.Web.Models;

namespace Data.Web.Services.Foundations;

internal sealed class DataSetService(IDataSetBroker dataSetBroker)
    : IDataSetService
{
    public Task<DataEntitySet[]> GetEntitySetsAsync(CancellationToken cancellationToken)
    {
        EnsureAuthenticated();

        return dataSetBroker.SelectEntitySetsAsync(cancellationToken:cancellationToken);
    }

    public Task<DataRows> GetRowsAsync(
        string entitySet,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        EnsureAuthenticated();

        return dataSetBroker.SelectRowsAsync(
entitySet:            entitySet,
skip:            Math.Max(val1:skip, val2:0),
take:            Math.Clamp(value:take, min:1, max:500),
cancellationToken:            cancellationToken);
    }

    public Task<Dictionary<string, object>> CreateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken)
    {
        EnsureAuthenticated();

        return dataSetBroker.InsertRowAsync(entitySet:entitySet, values:values, cancellationToken:cancellationToken);
    }

    public Task<Dictionary<string, object>> UpdateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken)
    {
        EnsureAuthenticated();

        return dataSetBroker.UpdateRowAsync(entitySet:entitySet, values:values, cancellationToken:cancellationToken);
    }

    public Task DeleteRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken)
    {
        EnsureAuthenticated();

        return dataSetBroker.DeleteRowAsync(entitySet:entitySet, values:values, cancellationToken:cancellationToken);
    }

    private void EnsureAuthenticated()
    {
        string ssoUserId = dataSetBroker.GetCurrentSsoUserId();

        if (string.IsNullOrWhiteSpace(value:ssoUserId) || ssoUserId == "Guest")
            throw new SecurityException("Authentication is required.");
    }
}