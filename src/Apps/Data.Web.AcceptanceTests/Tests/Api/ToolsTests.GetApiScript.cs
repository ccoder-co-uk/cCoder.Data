using FluentAssertions;

namespace Data.Web.AcceptanceTests.Tests.Api;

public sealed partial class ToolsTests
{
    [Fact]
    public async Task ShouldServeApiScript()
    {
        HttpResponseMessage response = await client.GetAsync("/tools/api.js");

        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("/Api/Account/Login");
        content.Should().Contain("Authorization");
    }
}
