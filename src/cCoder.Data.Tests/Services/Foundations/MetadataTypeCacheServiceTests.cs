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

        Action act = () => service.Set(scope, [CreateTypeSetPayload("App")]);

        act.Should().Throw<ValidationException>().WithMessage("Scope is required.");
    }

    [Fact]
    public void ShouldThrowValidationExceptionOnSetWhenTypeSetsAreNull()
    {
        var service = CreateService();

        Action act = () => service.Set("cms", null);

        act.Should().Throw<ValidationException>().WithMessage("Type sets are required.");
    }

    [Fact]
    public void ShouldThrowValidationExceptionOnSetWhenTypeSetsContainNull()
    {
        var service = CreateService();
        IEnumerable<string> typeSetPayloads = [CreateTypeSetPayload("App"), null];

        Action act = () => service.Set("cms", typeSetPayloads);

        act.Should()
            .Throw<ValidationException>()
            .WithMessage("Type sets contain invalid values.");
    }

    [Fact]
    public void ShouldSetAndGetByScopeWhenArgumentsAreValid()
    {
        var service = CreateService();
        string[] expected = [CreateTypeSetPayload("App"), CreateTypeSetPayload("Page")];

        service.Set("cms", expected);

        string[] actual = service.Get("cms");

        actual.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Fact]
    public void ShouldReturnAllCachedSets()
    {
        var service = CreateService();
        string[] cmsSets = [CreateTypeSetPayload("App")];
        string[] workflowSets = [CreateTypeSetPayload("Flow")];

        service.Set("cms", cmsSets);
        service.Set("workflow", workflowSets);

        string[] actual = service.GetAll();

        actual.Should().BeEquivalentTo([.. cmsSets, .. workflowSets], options => options.WithStrictOrdering());
    }

    [Fact]
    public void ShouldContainScopeWhenScopeIsCached()
    {
        var service = CreateService();
        service.Set("cms", [CreateTypeSetPayload("App")]);

        bool actual = service.Contains("cms");

        actual.Should().BeTrue();
    }

    [Fact]
    public void ShouldClearScopeWhenScopeIsCached()
    {
        var service = CreateService();
        service.Set("cms", [CreateTypeSetPayload("App")]);

        service.Clear("cms");

        service.Contains("cms").Should().BeFalse();
        service.Get("cms").Should().BeEmpty();
    }

    private static MetadataTypeCacheService CreateService() => new(new MetadataTypeCacheBroker());

    private static string CreateTypeSetPayload(string name) => $"{{\"Name\":\"{name}\"}}";
}

