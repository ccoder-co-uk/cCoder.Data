// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Models.Exceptions;

internal sealed class ServiceValidationException(Exception innerException)
    : System.ComponentModel.DataAnnotations.ValidationException(
        innerException.Message,
        innerException)
{
}