using cCoder.Data.Brokers.Caching;
using FluentAssertions;
using Xunit;

namespace cCoder.Data.Tests.Brokers.Caching;

public class MetadataTypeCacheBrokerTests
{
    [Fact]
    public void ShouldSetGetGetAllContainAndClearByScope()
    {
        var broker = new MetadataTypeCacheBroker();
        string[] typeSetPayloads =
        [
            "{\"Name\":\"App\"}",
            "{\"Name\":\"Page\"}",
        ];

        broker.Set("cms", typeSetPayloads);

        broker.Contains("cms").Should().BeTrue();
        broker.Get("cms").Should().BeEquivalentTo(typeSetPayloads, options => options.WithStrictOrdering());
        broker.GetAll().Should().BeEquivalentTo(typeSetPayloads, options => options.WithStrictOrdering());

        broker.Clear("cms");

        broker.Contains("cms").Should().BeFalse();
        broker.Get("cms").Should().BeEmpty();
        broker.GetAll().Should().BeEmpty();
    }

    [Fact]
    public void ShouldReturnEmptySetWhenScopeHasNotBeenCached()
    {
        var broker = new MetadataTypeCacheBroker();

        broker.Get("missing").Should().BeEmpty();
    }
}

