// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace Data.Web.Services.Foundations;

internal sealed partial class DataEntitySetService
{
    private static void Validate(params object[] inputs)
    {
        if (inputs.Any(predicate: input => input is null))
        {
            throw new ArgumentNullException(nameof(inputs));
        }
    }

    private static void ValidateEntitySetsOnGet(CancellationToken cancellationToken) =>
        Validate(inputs: cancellationToken);
}