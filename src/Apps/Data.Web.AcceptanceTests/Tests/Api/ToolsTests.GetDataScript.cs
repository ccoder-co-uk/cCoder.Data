// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using FluentAssertions;

namespace Data.Web.AcceptanceTests.Tests.Api;

public sealed partial class ToolsTests
{
    [Fact]
    public async Task ShouldServeDataScript()
    {
        HttpResponseMessage response = await client.GetAsync(requestUri:"/tools/data.js");

        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();
        content.Should().Contain(expected:"/Api/Data/EntitySets");
        content.Should().Contain(expected:"Create");
    }
}