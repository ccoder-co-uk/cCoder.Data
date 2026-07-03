using System.Text.Json;
using Data.Web.Models;

namespace Data.Web.Services.Foundations;

public interface IDataSetService
{
    Task<DataEntitySet[]> GetEntitySetsAsync(CancellationToken cancellationToken);

    Task<DataRows> GetRowsAsync(
        string entitySet,
        int skip,
        int take,
        CancellationToken cancellationToken);

    Task<Dictionary<string, object>> CreateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken);

    Task<Dictionary<string, object>> UpdateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken);

    Task DeleteRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken);
}
