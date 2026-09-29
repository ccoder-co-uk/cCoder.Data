// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Services.Foundations;

internal sealed partial class CommonObjectStorageService
{
    private static void ValidateCommonObjectsOnGet(object[] inputs) =>
        Validate(inputs: inputs);

    private static void ValidateCommonObjectOnAdd(object[] inputs) =>
        Validate(inputs: inputs);

    private static void ValidateCommonObjectOnUpdate(object[] inputs) =>
        Validate(inputs: inputs);

    private static void ValidateCommonObjectOnDelete(object[] inputs) =>
        Validate(inputs: inputs);

    private static void Validate(params object[] inputs)
    {
        if (inputs.Any(predicate: input => input is null))
        {
            throw new ArgumentNullException(nameof(inputs));
        }
    }
}