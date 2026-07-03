using FluentAssertions;

namespace Data.Web.AcceptanceTests.Tests.Api;

public sealed partial class RootTests
{
    [Fact]
    public async Task ShouldRedirectToTools()
    {
        HttpResponseMessage response = await client.GetAsync("/");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Redirect);
        response.Headers.Location?.ToString().Should().Be("/tools/index.html");
    }
}
