using System.Net;
using System.Text.Json;
using Data.Web.AcceptanceTests.Infrastructure;
using FluentAssertions;

namespace Data.Web.AcceptanceTests.Tests.Api;

[Collection(WebAcceptanceCollection.Name)]
public sealed partial class BaselineTests(WebAcceptanceFixture fixture)
{
    private readonly HttpClient client = fixture.Client;

    private async Task<JsonElement> GetBaselineAsync()
    {
        using HttpResponseMessage response = await client.GetAsync("/Api/Data/Baseline");
        string content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK, content);
        return JsonDocument.Parse(content).RootElement.Clone();
    }
}
