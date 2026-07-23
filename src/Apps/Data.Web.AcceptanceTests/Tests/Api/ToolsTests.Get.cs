// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using FluentAssertions;

namespace Data.Web.AcceptanceTests.Tests.Api;

public sealed partial class ToolsTests
{
    [Fact]
    public async Task ShouldServeToolsUi()
    {
        HttpResponseMessage response = await client.GetAsync(requestUri:"/tools/index.html");

        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();
        content.Should().Contain(expected:"Data");
        content.Should().Contain(expected:"data-section-tabs");
        content.Should().Contain(expected:"/tools/data.js");
        content.Should().Contain(expected:"Login required");
    }
}