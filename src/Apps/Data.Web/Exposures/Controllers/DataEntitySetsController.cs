// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Models.Exceptions;
using Data.Web.Services.Foundations;
using Microsoft.AspNetCore.Mvc;

namespace Data.Web.Exposures.Controllers;

[ApiController]
[Route("Api/Data/EntitySets")]
public sealed class DataEntitySetsController(IDataEntitySetService dataEntitySetService)
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