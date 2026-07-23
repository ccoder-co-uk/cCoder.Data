// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using cCoder.Data.Brokers.Caching;
using cCoder.Data.Services.Foundations;
using FluentAssertions;
using Xunit;

namespace cCoder.Data.Tests.Services.Foundations;

public sealed partial class MetadataTypeCacheServiceTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ShouldThrowValidationExceptionOnSetWhenScopeIsInvalid(string scope)
    {
        // Given
        MetadataTypeCacheService service = CreateMetadataTypeCacheService();
        string[] typeSetPayloads = CreateTypeSetPayloads(names: ["App"]);

        // When
        Action setAction = () => service.Set(
            scope: scope,
            typeSetPayloads: typeSetPayloads);

        // Then
        setAction
            .Should()
            .Throw<ValidationException>()
            .WithMessage(expectedWildcardPattern: "Scope is required.");
    }

    [Fact]
    public void ShouldThrowValidationExceptionOnSetWhenTypeSetsAreNull()
    {
        // Given
        MetadataTypeCacheService service = CreateMetadataTypeCacheService();

        // When
        Action setAction = () => service.Set(
            scope: "cms",
            typeSetPayloads: null);

        // Then
        setAction
            .Should()
            .Throw<ValidationException>();
    }

    [Fact]
    public void ShouldThrowValidationExceptionOnSetWhenTypeSetsContainNull()
    {
        // Given
        MetadataTypeCacheService service = CreateMetadataTypeCacheService();
        string[] typeSetPayloads = [CreateTypeSetPayload(name: "App"), null];

        // When
        Action setAction = () => service.Set(
            scope: "cms",
            typeSetPayloads: typeSetPayloads);

        // Then
        setAction
            .Should()
            .Throw<ValidationException>()
            .WithMessage(expectedWildcardPattern: "Type sets contain invalid values.");
    }

    [Fact]
    public void ShouldSetAndGetByScopeWhenArgumentsAreValid()
    {
        // Given
        MetadataTypeCacheBroker broker = CreateMetadataTypeCacheBroker();
        MetadataTypeCacheService service = CreateMetadataTypeCacheService(broker: broker);
        string[] expectedPayloads = CreateTypeSetPayloads(names: ["App", "Page"]);

        // When
        service.Set(
            scope: "cms",
            typeSetPayloads: expectedPayloads);

        string[] actualPayloads = service.Get(scope: "cms");

        // Then
        actualPayloads
            .Should()
            .BeEquivalentTo(
                expectation: expectedPayloads,
                config: options => options.WithStrictOrdering());
    }

    [Fact]
    public void ShouldReturnAllCachedSets()
    {
        // Given
        MetadataTypeCacheBroker broker = CreateMetadataTypeCacheBroker();
        MetadataTypeCacheService service = CreateMetadataTypeCacheService(broker: broker);
        string[] cmsPayloads = CreateTypeSetPayloads(names: ["App"]);
        string[] workflowPayloads = CreateTypeSetPayloads(names: ["Flow"]);

        service.Set(
            scope: "cms",
            typeSetPayloads: cmsPayloads);

        service.Set(
            scope: "workflow",
            typeSetPayloads: workflowPayloads);

        // When
        string[] actualPayloads = service.GetAll();

        // Then
        actualPayloads
            .Should()
            .BeEquivalentTo(
                expectation: [.. cmsPayloads, .. workflowPayloads],
                config: options => options.WithStrictOrdering());
    }

    [Fact]
    public void ShouldContainScopeWhenScopeIsCached()
    {
        // Given
        MetadataTypeCacheBroker broker = CreateMetadataTypeCacheBroker();
        MetadataTypeCacheService service = CreateMetadataTypeCacheService(broker: broker);
        string[] typeSetPayloads = CreateTypeSetPayloads(names: ["App"]);

        service.Set(
            scope: "cms",
            typeSetPayloads: typeSetPayloads);

        // When
        bool containsScope = service.Contains(scope: "cms");

        // Then
        containsScope
            .Should()
            .BeTrue();
    }

    [Fact]
    public void ShouldClearScopeWhenScopeIsCached()
    {
        // Given
        MetadataTypeCacheBroker broker = CreateMetadataTypeCacheBroker();
        MetadataTypeCacheService service = CreateMetadataTypeCacheService(broker: broker);
        string[] typeSetPayloads = CreateTypeSetPayloads(names: ["App"]);

        service.Set(
            scope: "cms",
            typeSetPayloads: typeSetPayloads);

        // When
        service.Clear(scope: "cms");

        // Then
        service
            .Contains(scope: "cms")
            .Should()
            .BeFalse();

        service
            .Get(scope: "cms")
            .Should()
            .BeEmpty();
    }

    private static MetadataTypeCacheBroker CreateMetadataTypeCacheBroker() =>
        new MetadataTypeCacheBroker();

    private static MetadataTypeCacheService CreateMetadataTypeCacheService(
        IMetadataTypeCacheBroker broker = null) =>
        new MetadataTypeCacheService(
            broker: broker ?? CreateMetadataTypeCacheBroker());

    private static string CreateTypeSetPayload(string name) =>
        $"{{\"Name\":\"{name}\"}}";

    private static string[] CreateTypeSetPayloads(string[] names) =>
        names
            .Select(selector: CreateTypeSetPayload)
            .ToArray();
}