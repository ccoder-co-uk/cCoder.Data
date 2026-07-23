// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Dependencies;
using System.ComponentModel.DataAnnotations;

namespace cCoder.Data.Services.Foundations;

internal partial class MetadataTypeCacheService
{
    private static void Validate(params object[] inputs) =>
        ValidationRulesEngine.Validate(inputs: inputs);

    private static void ValidateScope(string scope)
    {
        if (string.IsNullOrWhiteSpace(value: scope))
        {
            throw new ValidationException("Scope is required.");
        }

        ValidationRulesEngine.Validate(inputs: scope);
    }

    private static void ValidateTypeSetPayloads(IEnumerable<string> typeSetPayloads)
    {
        if (typeSetPayloads is null)
        {
            throw new ValidationException("Type sets are required.");
        }

        ValidationRulesEngine.Validate(inputs: typeSetPayloads);

        if (typeSetPayloads.Any(predicate: typeSetPayload => typeSetPayload is null))
        {
            throw new ValidationException("Type sets contain invalid values.");
        }
    }
}