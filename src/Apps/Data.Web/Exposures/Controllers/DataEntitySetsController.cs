// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Brokers.Loggings;
using Data.Web.Models.Exceptions;
using Data.Web.Services.Foundations;
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

            return Ok(value: entitySets);
        }
        catch (ServiceValidationException exception)
        {
            loggingBroker.LogError(exception: exception, message: "Controller request failed.");

            return BadRequest(error: "The data request is invalid.");
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
    }
}