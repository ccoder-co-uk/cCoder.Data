// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Brokers.Loggings;
using Data.Web.Models.Exceptions;
using Data.Web.Services.Foundations;
using Microsoft.AspNetCore.Mvc;

namespace Data.Web.Exposures.Controllers;

[ApiController]
[Route("Api/Data")]
public sealed class DataRowsController(IDataRowManager dataRowService,
    ILoggingBroker loggingBroker)
    : ControllerBase
{
    [HttpGet("{entitySet}")]
    public async ValueTask<IActionResult> GetRowsAsync(
        string entitySet,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 100,
        CancellationToken cancellationToken = default)
    {
        try
        {
            object rows = await dataRowService.GetRowsAsync(
                entitySet: entitySet,
                skip: skip,
                take: take,
                cancellationToken: cancellationToken);

            return Ok(value: rows);
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

    [HttpPost("{entitySet}")]
    public async ValueTask<IActionResult> PostRowAsync(
        string entitySet,
        [FromBody] Dictionary<string, object> values,
        CancellationToken cancellationToken)
    {
        try
        {
            object savedRow = await dataRowService.AddRowAsync(
                entitySet: entitySet,
                newValues: values,
                cancellationToken: cancellationToken);

            return StatusCode(
                statusCode: StatusCodes.Status201Created,
                value: savedRow);
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

    [HttpPut("{entitySet}")]
    public async ValueTask<IActionResult> PutRowAsync(
        string entitySet,
        [FromBody] Dictionary<string, object> values,
        CancellationToken cancellationToken)
    {
        try
        {
            object updatedRow = await dataRowService.UpdateRowAsync(
                entitySet: entitySet,
                updatedValues: values,
                cancellationToken: cancellationToken);

            return Ok(value: updatedRow);
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

    [HttpDelete("{entitySet}")]
    public async ValueTask<IActionResult> DeleteRowAsync(
        string entitySet,
        [FromBody] Dictionary<string, object> values,
        CancellationToken cancellationToken)
    {
        try
        {
            await dataRowService.DeleteRowAsync(
                entitySet: entitySet,
                deletedValues: values,
                cancellationToken: cancellationToken);

            return NoContent();
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