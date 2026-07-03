using Data.Web.AcceptanceTests.Infrastructure;

namespace Data.Web.AcceptanceTests.Tests.Api;

[Collection(WebAcceptanceCollection.Name)]
public sealed partial class ToolsTests(WebAcceptanceFixture fixture)
{
    private readonly HttpClient client = fixture.Client;
}
