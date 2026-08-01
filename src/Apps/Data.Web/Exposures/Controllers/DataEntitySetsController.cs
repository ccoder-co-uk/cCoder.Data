// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Models.Exceptions;
using Data.Web.Services.Foundations;
using Microsoft.AspNetCore.Mvc;

namespace Data.Web.Exposures.Controllers;

[ApiController]
[Route("Api/Data/EntitySets")]
public sealed class DataEntitySetsController(IDataEntitySetManager dataEntitySetService)
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
        catch (ServiceValidationException)
        {
            return BadRequest(error: "The data request is invalid.");
        }
        catch (ServiceDependencyException)
        {
            return StatusCode(
                statusCode: StatusCodes.Status503ServiceUnavailable,
                value: "The data service is unavailable.");
        }
        catch (ServiceException)
        {
            return StatusCode(
                statusCode: StatusCodes.Status500InternalServerError,
                value: "The data operation failed.");
        }
    }
}