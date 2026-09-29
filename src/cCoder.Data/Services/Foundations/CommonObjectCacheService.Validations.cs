// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Services.Foundations;

internal sealed partial class CommonObjectCacheService
{
    private static void ValidateCommonObjectsOnSet(object[] inputs) =>
        Validate(inputs: inputs);

    private static void Validate(params object[] inputs)
    {
        if (inputs.Any(predicate: input => input is null))
        {
            throw new ArgumentNullException(nameof(inputs));
        }
    }
}