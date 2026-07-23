// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using FluentAssertions;

namespace Data.Web.AcceptanceTests.Tests.Api;

public sealed partial class ToolsTests
{
    [Fact]
    public async Task ShouldServeApiScript()
    {
        // Given
        const string requestUri = "/tools/api.js";

        // When
        HttpResponseMessage response = await client
            .GetAsync(requestUri: requestUri);

        // Then
        response.EnsureSuccessStatusCode();

        string content = await response.Content
            .ReadAsStringAsync();

        content
            .Should()
            .Contain(expected: "/Api/Account/Login");

        content
            .Should()
            .Contain(expected: "Authorization");
    }
}