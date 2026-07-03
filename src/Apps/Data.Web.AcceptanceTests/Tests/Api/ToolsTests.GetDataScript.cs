using FluentAssertions;

namespace Data.Web.AcceptanceTests.Tests.Api;

public sealed partial class ToolsTests
{
    [Fact]
    public async Task ShouldServeDataScript()
    {
        HttpResponseMessage response = await client.GetAsync("/tools/data.js");

        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("/Api/Data/EntitySets");
        content.Should().Contain("Create");
    }
}
