// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace cCoder.Data.Services.Foundations;

internal partial class MetadataTypeCacheService
{
    private static void TryCatch(Action operation)
    {
        try
        {
            operation();
        }
        catch (ValidationException innerException)
        {
            throw new ServiceValidationException(innerException);
        }
        catch (ArgumentException innerException)
        {
            throw new ServiceValidationException(innerException);
        }
        catch (InvalidOperationException innerException)
        {
            throw new ServiceDependencyException(innerException);
        }
        catch (Exception innerException)
        {
            throw new ServiceException(innerException);
        }
    }

    private static TResult TryCatch<TResult>(Func<TResult> operation)
    {
        try
        {
            return operation();
        }
        catch (ValidationException innerException)
        {
            throw new ServiceValidationException(innerException);
        }
        catch (ArgumentException innerException)
        {
            throw new ServiceValidationException(innerException);
        }
        catch (InvalidOperationException innerException)
        {
            throw new ServiceDependencyException(innerException);
        }
        catch (Exception innerException)
        {
            throw new ServiceException(innerException);
        }
    }
}