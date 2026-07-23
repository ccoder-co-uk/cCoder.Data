// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace Data.Web.AcceptanceTests.Infrastructure;

public sealed class WebAcceptanceFixture : IDisposable
{
    private readonly WebAcceptanceFactory factory = new();

    public HttpClient Client { get; }

    public WebAcceptanceFixture()
    {
        Client = factory.CreateClient(options:new()
        {
            AllowAutoRedirect = false
        });
    }

    public void Dispose()
    {
        Client.Dispose();
        factory.Dispose();
    }
}