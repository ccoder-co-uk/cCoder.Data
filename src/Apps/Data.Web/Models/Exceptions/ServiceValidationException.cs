// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;

namespace Data.Web.Models.Exceptions;

internal sealed class ServiceValidationException(Exception innerException)
    : Exception("A validation error occurred.", innerException)
{
}