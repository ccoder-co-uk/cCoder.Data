// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

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

        broker.Set(scope:"cms", typeSetPayloads:typeSetPayloads);

        broker.Contains(scope:"cms").Should().BeTrue();
        broker.Get("cms").Should().BeEquivalentTo(expectation:typeSetPayloads, config:options => options.WithStrictOrdering());
        broker.GetAll().Should().BeEquivalentTo(expectation:typeSetPayloads, config:options => options.WithStrictOrdering());

        broker.Clear(scope:"cms");

        broker.Contains(scope:"cms").Should().BeFalse();
        broker.Get(scope:"cms").Should().BeEmpty();
        broker.GetAll().Should().BeEmpty();
    }

    [Fact]
    public void ShouldReturnEmptySetWhenScopeHasNotBeenCached()
    {
        var broker = new MetadataTypeCacheBroker();

        broker.Get(scope:"missing").Should().BeEmpty();
    }
}