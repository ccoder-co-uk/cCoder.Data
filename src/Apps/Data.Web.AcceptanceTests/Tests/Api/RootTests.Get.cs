// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using FluentAssertions;

namespace Data.Web.AcceptanceTests.Tests.Api;

public sealed partial class RootTests
{
    [Fact]
    public async Task ShouldRedirectToTools()
    {
        HttpResponseMessage response = await client.GetAsync(requestUri:"/");

        response.StatusCode.Should().Be(expected:System.Net.HttpStatusCode.Redirect);
        response.Headers.Location?.ToString().Should().Be(expected:"/tools/index.html");
    }
}