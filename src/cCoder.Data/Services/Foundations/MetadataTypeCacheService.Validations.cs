// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Dependencies;

namespace cCoder.Data.Services.Foundations;

internal partial class MetadataTypeCacheService
{
    private static void Validate(params object[] inputs) =>
        ValidationRulesEngine.Validate(inputs: inputs);

    private static void ValidateScope(string scope)
    {
        ValidationRulesEngine.Validate(inputs: scope);

        if (string.IsNullOrWhiteSpace(value: scope))
        {
            throw new ArgumentException("Scope is required.", nameof(scope));
        }
    }

    private static void ValidateTypeSetPayloads(IEnumerable<string> typeSetPayloads)
    {
        ValidationRulesEngine.Validate(inputs: typeSetPayloads);

        if (typeSetPayloads.Any(predicate: typeSetPayload => typeSetPayload is null))
        {
            throw new ArgumentException(
                "Type sets contain invalid values.",
                nameof(typeSetPayloads));
        }
    }
}