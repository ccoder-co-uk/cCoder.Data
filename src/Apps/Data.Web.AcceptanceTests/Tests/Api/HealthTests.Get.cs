// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using FluentAssertions;

namespace Data.Web.AcceptanceTests.Tests.Api;

public sealed partial class HealthTests
{
    [Fact]
    public async Task ShouldReturnOk()
    {
        // Given
        const string requestUri = "/Health";

        // When
        HttpResponseMessage response = await client.GetAsync(requestUri: requestUri);

        // Then
        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();

        content
            .Should()
            .Be(expected: "OK");
    }
}