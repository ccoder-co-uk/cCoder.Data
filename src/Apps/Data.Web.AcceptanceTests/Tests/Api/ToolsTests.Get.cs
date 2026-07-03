using FluentAssertions;

namespace Data.Web.AcceptanceTests.Tests.Api;

public sealed partial class ToolsTests
{
    [Fact]
    public async Task ShouldServeToolsUi()
    {
        HttpResponseMessage response = await client.GetAsync("/tools/index.html");

        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Data");
        content.Should().Contain("data-section-tabs");
        content.Should().Contain("/tools/data.js");
        content.Should().Contain("Login required");
    }
}
