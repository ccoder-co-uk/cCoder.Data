// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;

namespace cCoder.Data.Models.Exceptions;

internal sealed class ServiceValidationException(Exception innerException)
    : System.ComponentModel.DataAnnotations.ValidationException(
        innerException.Message,
        innerException)
{
}