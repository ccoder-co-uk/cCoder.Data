// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace Data.Web.Services.Foundations;

internal sealed partial class DataRowService
{
    private static void Validate(params object[] inputs)
    {
        if (inputs.Any(predicate: input => input is null))
        {
            throw new ArgumentNullException(nameof(inputs));
        }
    }

    private static void ValidateRowsOnGet(
        string entitySet,
        int skip,
        int take,
        CancellationToken cancellationToken) =>
        Validate(inputs: [entitySet, skip, take, cancellationToken]);

    private static void ValidateRowOnAdd(
        string entitySet,
        Dictionary<string, object> newValues,
        CancellationToken cancellationToken) =>
        Validate(inputs: [entitySet, newValues, cancellationToken]);

    private static void ValidateRowOnUpdate(
        string entitySet,
        Dictionary<string, object> updatedValues,
        CancellationToken cancellationToken) =>
        Validate(inputs: [entitySet, updatedValues, cancellationToken]);

    private static void ValidateRowOnDelete(
        string entitySet,
        Dictionary<string, object> deletedValues,
        CancellationToken cancellationToken) =>
        Validate(inputs: [entitySet, deletedValues, cancellationToken]);
}