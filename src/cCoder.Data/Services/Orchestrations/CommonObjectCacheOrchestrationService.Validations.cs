// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Services.Orchestrations;

internal sealed partial class CommonObjectCacheOrchestrationService
{
    private static void ValidateCommonObjectsOnGet(object[] inputs) =>
        Validate(inputs: inputs);

    private static void ValidateCommonObjectOnAdd(object[] inputs) =>
        Validate(inputs: inputs);

    private static void ValidateCommonObjectOnUpdate(object[] inputs) =>
        Validate(inputs: inputs);

    private static void ValidateCommonObjectOnDelete(object[] inputs) =>
        Validate(inputs: inputs);

    private static void ValidateAllCommonObjectsOnDelete(object[] inputs) =>
        Validate(inputs: inputs);

    private static void ValidateCommonObjectsOnRefresh() =>
        Validate();

    private static void Validate(params object[] inputs)
    {
        if (inputs.Any(predicate: input => input is null))
        {
            throw new ArgumentNullException(nameof(inputs));
        }
    }
}