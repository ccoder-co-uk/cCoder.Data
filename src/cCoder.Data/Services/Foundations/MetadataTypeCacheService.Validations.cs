using System.ComponentModel.DataAnnotations;

namespace cCoder.Data.Services.Foundations;

internal partial class MetadataTypeCacheService
{
    private static void ValidateScope(string scope)
    {
        if (string.IsNullOrWhiteSpace(scope))
            throw new ValidationException("Scope is required.");
    }

    private static void ValidateTypeSetPayloads(IEnumerable<string> typeSetPayloads)
    {
        if (typeSetPayloads is null)
            throw new ValidationException("Type sets are required.");

        if (typeSetPayloads.Any(typeSetPayload => typeSetPayload is null))
            throw new ValidationException("Type sets contain invalid values.");
    }
}


