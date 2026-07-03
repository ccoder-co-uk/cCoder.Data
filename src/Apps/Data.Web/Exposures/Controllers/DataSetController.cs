using System.Text.Json;
using Data.Web.Models;
using Data.Web.Services.Orchestrations;
using Microsoft.AspNetCore.Mvc;

namespace Data.Web.Exposures.Controllers;

[ApiController]
[Route("Api/Data")]
public sealed class DataSetController(IDataSetOrchestrationService dataSetOrchestrationService)
    : ControllerBase
{
    [HttpGet("EntitySets")]
    public Task<DataEntitySet[]> GetEntitySetsAsync(CancellationToken cancellationToken) =>
        dataSetOrchestrationService.GetEntitySetsAsync(cancellationToken);

    [HttpGet("{entitySet}")]
    public Task<DataRows> GetRowsAsync(
        string entitySet,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 100,
        CancellationToken cancellationToken = default) =>
            dataSetOrchestrationService.GetRowsAsync(entitySet, skip, take, cancellationToken);

    [HttpPost("{entitySet}")]
    public Task<Dictionary<string, object>> PostRowAsync(
        string entitySet,
        [FromBody] Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
            dataSetOrchestrationService.CreateRowAsync(entitySet, values, cancellationToken);

    [HttpPut("{entitySet}")]
    public Task<Dictionary<string, object>> PutRowAsync(
        string entitySet,
        [FromBody] Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
            dataSetOrchestrationService.UpdateRowAsync(entitySet, values, cancellationToken);

    [HttpDelete("{entitySet}")]
    public Task DeleteRowAsync(
        string entitySet,
        [FromBody] Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken) =>
            dataSetOrchestrationService.DeleteRowAsync(entitySet, values, cancellationToken);
}
