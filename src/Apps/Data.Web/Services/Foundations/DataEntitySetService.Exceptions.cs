// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;
using System.Security;
using System.Security.Authentication;
using System.Threading.Tasks;
using Data.Web.Models.Exceptions;

namespace Data.Web.Services.Foundations;

internal sealed partial class DataEntitySetService
{
    private static async ValueTask<TResult> TryCatch<TResult>(
        Func<ValueTask<TResult>> operation)
    {
        try
        {
            return await operation();
        }
        catch (ArgumentException innerException)
        {
            throw new ServiceValidationException(innerException);
        }
        catch (InvalidOperationException innerException)
        {
            throw new ServiceDependencyException(innerException);
        }
        catch (AuthenticationException)
        {
            throw;
        }
        catch (UnauthorizedAccessException)
        {
            throw;
        }
        catch (SecurityException)
        {
            throw;
        }
        catch (Exception innerException)
        {
            throw new ServiceException(innerException);
        }
    }
}