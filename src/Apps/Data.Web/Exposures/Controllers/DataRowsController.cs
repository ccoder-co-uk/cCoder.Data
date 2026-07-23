// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Text.Json;
using Data.Web.Models.Exceptions;
using Data.Web.Services.Foundations;
using Microsoft.AspNetCore.Mvc;

namespace Data.Web.Exposures.Controllers;

[ApiController]
[Route("Api/Data")]
public sealed class DataRowsController(IDataRowService dataRowService)
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
            return BadRequest(error: exception.Message);
        }
        catch (ServiceDependencyException exception)
        {
            return Problem(detail: exception.Message);
        }
        catch (ServiceException exception)
        {
            return Problem(detail: exception.Message);
        }
    }

    [HttpPost("{entitySet}")]
    public async ValueTask<IActionResult> PostRowAsync(
        string entitySet,
        [FromBody] Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken)
    {
        try
        {
            object savedRow = await dataRowService.AddRowAsync(
                entitySet: entitySet,
                newValues: values,
                cancellationToken: cancellationToken);

            return Ok(value: savedRow);
        }
        catch (ServiceValidationException exception)
        {
            return BadRequest(error: exception.Message);
        }
        catch (ServiceDependencyException exception)
        {
            return Problem(detail: exception.Message);
        }
        catch (ServiceException exception)
        {
            return Problem(detail: exception.Message);
        }
    }

    [HttpPut("{entitySet}")]
    public async ValueTask<IActionResult> PutRowAsync(
        string entitySet,
        [FromBody] Dictionary<string, JsonElement> values,
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
            return BadRequest(error: exception.Message);
        }
        catch (ServiceDependencyException exception)
        {
            return Problem(detail: exception.Message);
        }
        catch (ServiceException exception)
        {
            return Problem(detail: exception.Message);
        }
    }

    [HttpDelete("{entitySet}")]
    public async ValueTask<IActionResult> DeleteRowAsync(
        string entitySet,
        [FromBody] Dictionary<string, JsonElement> values,
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
            return BadRequest(error: exception.Message);
        }
        catch (ServiceDependencyException exception)
        {
            return Problem(detail: exception.Message);
        }
        catch (ServiceException exception)
        {
            return Problem(detail: exception.Message);
        }
    }
}