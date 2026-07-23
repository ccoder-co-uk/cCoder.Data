// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Brokers.Caching;
using FluentAssertions;
using Xunit;

namespace cCoder.Data.Tests.Brokers.Caching;

public sealed partial class MetadataTypeCacheBrokerTests
{
    [Fact]
    public void ShouldSetGetGetAllContainAndClearByScope()
    {
        // Given
        MetadataTypeCacheBroker broker = CreateMetadataTypeCacheBroker();

        string[] typeSetPayloads =
        [
            "{\"Name\":\"App\"}",
            "{\"Name\":\"Page\"}",
        ];

        // When
        broker.Set(
            scope: "cms",
            typeSetPayloads: typeSetPayloads);

        // Then
        broker
            .Contains(scope: "cms")
            .Should()
            .BeTrue();

        broker
            .Get(scope: "cms")
            .Should()
            .BeEquivalentTo(
                expectation: typeSetPayloads,
                config: options => options.WithStrictOrdering());

        broker
            .GetAll()
            .Should()
            .BeEquivalentTo(
                expectation: typeSetPayloads,
                config: options => options.WithStrictOrdering());

        // When
        broker.Clear(scope: "cms");

        // Then
        broker
            .Contains(scope: "cms")
            .Should()
            .BeFalse();

        broker
            .Get(scope: "cms")
            .Should()
            .BeEmpty();

        broker
            .GetAll()
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void ShouldReturnEmptySetWhenScopeHasNotBeenCached()
    {
        // Given
        MetadataTypeCacheBroker broker = CreateMetadataTypeCacheBroker();

        // When
        string[] typeSetPayloads = broker.Get(scope: "missing");

        // Then
        typeSetPayloads
            .Should()
            .BeEmpty();
    }

    private static MetadataTypeCacheBroker CreateMetadataTypeCacheBroker() =>
        new MetadataTypeCacheBroker();
}