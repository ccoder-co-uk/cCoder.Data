namespace Data.Web.AcceptanceTests.Infrastructure;

[CollectionDefinition(Name)]
public sealed class WebAcceptanceCollection : ICollectionFixture<WebAcceptanceFixture>
{
    public const string Name = "Data Web acceptance";
}
