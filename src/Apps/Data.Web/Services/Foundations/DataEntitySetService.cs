// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Dependencies;
using Data.Web.Models;

namespace Data.Web.Services.Foundations;

internal sealed partial class DataEntitySetService(IDataSetBroker dataSetBroker)
    : IDataEntitySetService
{
    public ValueTask<DataEntitySet[]> GetEntitySetsAsync(
        CancellationToken cancellationToken) =>
        TryCatch(operation: async () =>
        {
            Validate(inputs: cancellationToken);
            ValidateAuthentication();

            return await dataSetBroker.SelectEntitySetsAsync(
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