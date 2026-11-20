// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Data.Web.Brokers.Loggings;
using Data.Web.Models.Exceptions;
using Data.Web.Services.Foundations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Data.Web.Exposures.Controllers;

[ApiController]
[Route("Api/Data/EntitySets")]
public sealed class DataEntitySetsController(IDataEntitySetService dataEntitySetService,
    ILoggingBroker loggingBroker)
    : ControllerBase
{
    [HttpGet]
    public async ValueTask<IActionResult> GetEntitySetsAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            object entitySets = await dataEntitySetService.GetEntitySetsAsync(
                cancellationToken: cancellationToken);

            IActionResult successResponse = Ok(value: entitySets);

            loggingBroker.LogInformation(message: "Controller request succeeded.");

            return successResponse;
        }
        catch (ServiceValidationException exception)
        {
            loggingBroker.LogWarning(message: exception.Message);

            return BadRequest(error: "The data request is invalid.");
        }
        catch (ArgumentException exception)
        {
            loggingBroker.LogWarning(message: exception.Message);

            return BadRequest(error: "The request is invalid.");
        }
        catch (System.Security.Authentication.AuthenticationException exception)
        {
            loggingBroker.LogWarning(message: exception.Message);

            return Unauthorized(value: "Authentication failed.");
        }
        catch (UnauthorizedAccessException exception)
        {
            loggingBroker.LogWarning(message: exception.Message);

            return StatusCode(
                statusCode: StatusCodes.Status403Forbidden,
                value: "Access is denied.");
        }
        catch (SecurityException exception)
        {
            loggingBroker.LogWarning(message: exception.Message);

            return StatusCode(
                statusCode: StatusCodes.Status403Forbidden,
                value: "Access is denied.");
        }
        catch (ServiceDependencyException exception)
        {
            loggingBroker.LogError(exception: exception, message: "Controller request failed.");

            return StatusCode(
                statusCode: StatusCodes.Status503ServiceUnavailable,
                value: "The data service is unavailable.");
        }
        catch (ServiceException exception)
        {
            loggingBroker.LogError(exception: exception, message: "Controller request failed.");

            return StatusCode(
                statusCode: StatusCodes.Status500InternalServerError,
                value: "The data operation failed.");
        }
        catch (Exception exception)
        {
            loggingBroker.LogError(exception: exception, message: "Controller request failed.");

            return StatusCode(
                statusCode: StatusCodes.Status500InternalServerError,
                value: "The data operation failed.");
        }
    }
}