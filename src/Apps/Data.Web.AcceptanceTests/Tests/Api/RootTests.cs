// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.AcceptanceTests.Infrastructure;

namespace Data.Web.AcceptanceTests.Tests.Api;

[Collection(WebAcceptanceCollection.Name)]
public sealed partial class RootTests(WebAcceptanceFixture fixture)
{
    private readonly HttpClient client = fixture.Client;
}