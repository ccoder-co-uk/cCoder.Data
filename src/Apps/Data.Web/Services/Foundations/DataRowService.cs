// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Brokers;
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
            ValidateRowsOnGet(
                entitySet: entitySet,
                skip: skip,
                take: take,
                cancellationToken: cancellationToken);

            ValidateAuthentication();

            int actualSkip = Math.Max(val1: skip, val2: 0);
            int actualTake = Math.Clamp(value: take, min: 1, max: 500);

            (string EntitySet, Dictionary<string, object>[] Rows) result =
                await dataSetBroker.SelectRowsAsync(
                entitySet: entitySet,
                skip: actualSkip,
                take: actualTake,
                cancellationToken: cancellationToken);

            return new DataRows
            {
                EntitySet = result.EntitySet,
                Skip = actualSkip,
                Take = actualTake,
                Rows = result.Rows
            };
        });

    public ValueTask<Dictionary<string, object>> AddRowAsync(
        string entitySet,
        Dictionary<string, object> newValues,
        CancellationToken cancellationToken) =>
        TryCatch(operation: async () =>
        {
            ValidateRowOnAdd(
                entitySet: entitySet,
                newValues: newValues,
                cancellationToken: cancellationToken);

            ValidateAuthentication();

            return await dataSetBroker.InsertRowAsync(
                entitySet: entitySet,
                values: newValues,
                cancellationToken: cancellationToken);
        });

    public ValueTask<Dictionary<string, object>> UpdateRowAsync(
        string entitySet,
        Dictionary<string, object> updatedValues,
        CancellationToken cancellationToken) =>
        TryCatch(operation: async () =>
        {
            ValidateRowOnUpdate(
                entitySet: entitySet,
                updatedValues: updatedValues,
                cancellationToken: cancellationToken);

            ValidateAuthentication();

            return await dataSetBroker.UpdateRowAsync(
                entitySet: entitySet,
                values: updatedValues,
                cancellationToken: cancellationToken);
        });

    public ValueTask DeleteRowAsync(
        string entitySet,
        Dictionary<string, object> deletedValues,
        CancellationToken cancellationToken) =>
        TryCatch(operation: async () =>
        {
            ValidateRowOnDelete(
                entitySet: entitySet,
                deletedValues: deletedValues,
                cancellationToken: cancellationToken);

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