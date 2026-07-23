// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Dependencies;
using System.Text.Json;

namespace Data.Web.Services.Foundations;

internal sealed partial class DataRowService
{
    private static void Validate(params object[] inputs) =>
        ValidationRulesEngine.Validate(inputs: inputs);

    private static void ValidateRowsOnGet(
        string entitySet,
        int skip,
        int take,
        CancellationToken cancellationToken) =>
        Validate(inputs: [entitySet, skip, take, cancellationToken]);

    private static void ValidateRowOnAdd(
        string entitySet,
        Dictionary<string, JsonElement> newValues,
        CancellationToken cancellationToken) =>
        Validate(inputs: [entitySet, newValues, cancellationToken]);

    private static void ValidateRowOnUpdate(
        string entitySet,
        Dictionary<string, JsonElement> updatedValues,
        CancellationToken cancellationToken) =>
        Validate(inputs: [entitySet, updatedValues, cancellationToken]);

    private static void ValidateRowOnDelete(
        string entitySet,
        Dictionary<string, JsonElement> deletedValues,
        CancellationToken cancellationToken) =>
        Validate(inputs: [entitySet, deletedValues, cancellationToken]);
}