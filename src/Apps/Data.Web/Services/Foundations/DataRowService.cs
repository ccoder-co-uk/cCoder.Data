// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Text.Json;
using Data.Web.Dependencies;
using Data.Web.Models;

namespace Data.Web.Services.Foundations;

internal sealed partial class DataRowService(IDataSetBroker dataSetBroker)
    : IDataRowService
{
    public ValueTask<DataRows> GetRowsAsync(
        string entitySet,
        int skip,
        int take,
        CancellationToken cancellationToken) =>
        TryCatch(operation: async () =>
        {
            Validate(inputs: [entitySet, skip, take, cancellationToken]);
            ValidateAuthentication();

            return await dataSetBroker.SelectRowsAsync(
                entitySet: entitySet,
                skip: Math.Max(val1: skip, val2: 0),
                take: Math.Clamp(value: take, min: 1, max: 500),
                cancellationToken: cancellationToken);
        });

    public ValueTask<Dictionary<string, object>> AddRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> newValues,
        CancellationToken cancellationToken) =>
        TryCatch(operation: async () =>
        {
            Validate(inputs: [entitySet, newValues, cancellationToken]);
            ValidateAuthentication();

            return await dataSetBroker.InsertRowAsync(
                entitySet: entitySet,
                values: newValues,
                cancellationToken: cancellationToken);
        });

    public ValueTask<Dictionary<string, object>> UpdateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> updatedValues,
        CancellationToken cancellationToken) =>
        TryCatch(operation: async () =>
        {
            Validate(inputs: [entitySet, updatedValues, cancellationToken]);
            ValidateAuthentication();

            return await dataSetBroker.UpdateRowAsync(
                entitySet: entitySet,
                values: updatedValues,
                cancellationToken: cancellationToken);
        });

    public ValueTask DeleteRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> deletedValues,
        CancellationToken cancellationToken) =>
        TryCatch(operation: async () =>
        {
            Validate(inputs: [entitySet, deletedValues, cancellationToken]);
            ValidateAuthentication();

            await dataSetBroker.DeleteRowAsync(
                entitySet: entitySet,
                values: deletedValues,
                cancellationToken: cancellationToken);
        });

    private void ValidateAuthentication()
    {
        string ssoUserId = dataSetBroker.GetCurrentSsoUserId();

        if (string.IsNullOrWhiteSpace(value: ssoUserId) || ssoUserId == "Guest")
        {
            throw new UnauthorizedAccessException("Authentication is required.");
        }
    }
}