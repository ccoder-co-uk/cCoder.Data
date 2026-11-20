// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Data.Web.AcceptanceTests.Infrastructure;
using FluentAssertions;
using Xunit;

namespace Data.Web.AcceptanceTests.Tests.Api;

[Collection(WebAcceptanceCollection.Name)]
public sealed partial class DataAuthorizationTests(WebAcceptanceFixture fixture)
{
    private readonly HttpClient client = fixture.Client;

    [Theory]
    [InlineData("/Api/Data/EntitySets")]
    [InlineData("/Api/Data/Customers")]
    public async Task AnonymousDataRequest_ShouldReturnForbidden(string requestUri)
    {
        // Given

        // When
        HttpResponseMessage response = await client.GetAsync(
            requestUri: requestUri);

        // Then
        response.StatusCode
            .Should()
            .Be(expected: HttpStatusCode.Forbidden);
    }
}