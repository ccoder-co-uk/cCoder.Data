// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Brokers;
using Data.Web.Models;

namespace Data.Web.Services.Foundations;

internal sealed partial class DataEntitySetService(IDataSetBroker dataSetBroker)
    : IDataEntitySetService
{
    public ValueTask<DataEntitySet[]> GetEntitySetsAsync(
        CancellationToken cancellationToken) =>
        TryCatch(operation: async () =>
        {
            ValidateEntitySetsOnGet(cancellationToken: cancellationToken);
            ValidateAuthentication();

            return (await dataSetBroker.SelectEntitySetsAsync(
                    cancellationToken: cancellationToken))
                .Select(selector: item => new DataEntitySet
                {
                    Name = item.Name,
                    DisplayName = item.DisplayName,
                    ClrType = item.ClrType,
                    Table = item.Table,
                    KeyProperties = item.KeyProperties,
                    Properties = item.Properties
                        .Select(selector: property => new DataProperty
                        {
                            Name = property.Name,
                            Type = property.Type,
                            IsKey = property.IsKey,
                            IsNullable = property.IsNullable,
                            CanCreate = property.CanCreate,
                            CanUpdate = property.CanUpdate,
                            IsLongText = property.IsLongText
                        })
                        .ToArray()
                })
                .ToArray();
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