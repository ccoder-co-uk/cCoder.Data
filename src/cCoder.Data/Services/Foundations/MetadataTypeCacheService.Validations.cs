// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace cCoder.Data.Services.Foundations;

internal partial class MetadataTypeCacheService
{
    private static void Validate(params object[] inputs)
    {
        if (inputs.Any(predicate: input => input is null))
        {
            throw new ArgumentNullException(nameof(inputs));
        }
    }

    private static void ValidateMetadataTypeCacheOnSet(
        string scope,
        IEnumerable<string> typeSetPayloads)
    {
        ValidateScope(scope: scope);
        ValidateTypeSetPayloads(typeSetPayloads: typeSetPayloads);
    }

    private static void ValidateScope(string scope)
    {
        if (string.IsNullOrWhiteSpace(value: scope))
        {
            throw new ValidationException("Scope is required.");
        }

        Validate(inputs: scope);
    }

    private static void ValidateTypeSetPayloads(IEnumerable<string> typeSetPayloads)
    {
        if (typeSetPayloads is null)
        {
            throw new ValidationException("Type sets are required.");
        }

        Validate(inputs: typeSetPayloads);

        if (typeSetPayloads.Any(predicate: typeSetPayload => typeSetPayload is null))
        {
            throw new ValidationException("Type sets contain invalid values.");
        }
    }
}