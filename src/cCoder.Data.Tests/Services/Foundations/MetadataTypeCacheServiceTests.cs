// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using cCoder.Data.Brokers.Caching;
using cCoder.Data.Services.Foundations;
using FluentAssertions;
using Xunit;

namespace cCoder.Data.Tests.Services.Foundations;

public class MetadataTypeCacheServiceTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ShouldThrowValidationExceptionOnSetWhenScopeIsInvalid(string scope)
    {
        var service = CreateService();

        Action act = () => service.Set(scope:scope, typeSetPayloads:[CreateTypeSetPayload("App")]);

        act.Should().Throw<ValidationException>().WithMessage(expectedWildcardPattern:"Scope is required.");
    }

    [Fact]
    public void ShouldThrowValidationExceptionOnSetWhenTypeSetsAreNull()
    {
        var service = CreateService();

        Action act = () => service.Set(scope:"cms", typeSetPayloads:null);

        act.Should().Throw<ValidationException>().WithMessage(expectedWildcardPattern:"Type sets are required.");
    }

    [Fact]
    public void ShouldThrowValidationExceptionOnSetWhenTypeSetsContainNull()
    {
        var service = CreateService();
        IEnumerable<string> typeSetPayloads = [CreateTypeSetPayload(name:"App"), null];

        Action act = () => service.Set(scope:"cms", typeSetPayloads:typeSetPayloads);

        act.Should()
            .Throw<ValidationException>()
            .WithMessage(expectedWildcardPattern:"Type sets contain invalid values.");
    }

    [Fact]
    public void ShouldSetAndGetByScopeWhenArgumentsAreValid()
    {
        var service = CreateService();
        string[] expected = [CreateTypeSetPayload(name:"App"), CreateTypeSetPayload(name:"Page")];

        service.Set(scope:"cms", typeSetPayloads:expected);

        string[] actual = service.Get(scope:"cms");

        actual.Should().BeEquivalentTo(expectation:expected, config:options => options.WithStrictOrdering());
    }

    [Fact]
    public void ShouldReturnAllCachedSets()
    {
        var service = CreateService();
        string[] cmsSets = [CreateTypeSetPayload(name:"App")];
        string[] workflowSets = [CreateTypeSetPayload(name:"Flow")];

        service.Set(scope:"cms", typeSetPayloads:cmsSets);
        service.Set(scope:"workflow", typeSetPayloads:workflowSets);

        string[] actual = service.GetAll();

        actual.Should().BeEquivalentTo(expectation:[.. cmsSets, .. workflowSets], config:options => options.WithStrictOrdering());
    }

    [Fact]
    public void ShouldContainScopeWhenScopeIsCached()
    {
        var service = CreateService();
        service.Set(scope:"cms", typeSetPayloads:[CreateTypeSetPayload("App")]);

        bool actual = service.Contains(scope:"cms");

        actual.Should().BeTrue();
    }

    [Fact]
    public void ShouldClearScopeWhenScopeIsCached()
    {
        var service = CreateService();
        service.Set(scope:"cms", typeSetPayloads:[CreateTypeSetPayload("App")]);

        service.Clear(scope:"cms");

        service.Contains(scope:"cms").Should().BeFalse();
        service.Get(scope:"cms").Should().BeEmpty();
    }

    private static MetadataTypeCacheService CreateService() =>
        new(new MetadataTypeCacheBroker());

    private static string CreateTypeSetPayload(string name) =>
        $"{{\"Name\":\"{name}\"}}";
}