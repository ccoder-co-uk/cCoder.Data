// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System;
using cCoder.Data.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace cCoder.Data.Services.Foundations;

internal sealed partial class CommonObjectStorageService
{
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
        catch (DbUpdateConcurrencyException innerException)
        {
            throw new ServiceDependencyException(innerException);
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

    private static async ValueTask<TResult> TryCatch<TResult>(
        Func<ValueTask<TResult>> operation,
        bool isValueTask)
    {
        try
        {
            return await operation();
        }
        catch (ValidationException innerException)
        {
            throw new ServiceValidationException(innerException);
        }
        catch (ArgumentException innerException)
        {
            throw new ServiceValidationException(innerException);
        }
        catch (DbUpdateConcurrencyException innerException)
        {
            throw new ServiceDependencyException(innerException);
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