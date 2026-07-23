// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Exposures;
using cCoder.Data.Services.Foundations;
using FluentAssertions;
using Moq;
using Xunit;

namespace cCoder.Data.Tests.Exposures.Caching;

public class MetadataTypeCacheTests
{
    private readonly Mock<IMetadataTypeCacheService> serviceMock;
    private readonly MetadataTypeCache cache;

    public MetadataTypeCacheTests()
    {
        serviceMock = new Mock<IMetadataTypeCacheService>(MockBehavior.Strict);
        cache = new MetadataTypeCache(serviceMock.Object);
    }

    [Fact]
    public void ShouldDelegateSet()
    {
        string[] typeSetPayloads = ["{\"Name\":\"App\"}"];

        serviceMock.Setup(expression:service => service.Set(scope:"cms", typeSetPayloads:typeSetPayloads));

        cache.Set(scope:"cms", typeSetPayloads:typeSetPayloads);

        serviceMock.Verify(expression:service => service.Set(scope:"cms", typeSetPayloads:typeSetPayloads), times:Times.Once);
        serviceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldDelegateGet()
    {
        string[] expected = ["{\"Name\":\"App\"}"];

        serviceMock.Setup(expression:service => service.Get("cms")).Returns(value:expected);

        string[] actual = cache.Get(scope:"cms");

        actual.Should().BeSameAs(expected:expected);
        serviceMock.Verify(expression:service => service.Get(scope:"cms"), times:Times.Once);
        serviceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldDelegateGetAll()
    {
        string[] expected =
        [
            "{\"Name\":\"App\"}",
            "{\"Name\":\"Page\"}",
        ];

        serviceMock.Setup(expression:service => service.GetAll()).Returns(value:expected);

        string[] actual = cache.GetAll();

        actual.Should().BeSameAs(expected:expected);
        serviceMock.Verify(expression:service => service.GetAll(), times:Times.Once);
        serviceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldDelegateContains()
    {
        serviceMock.Setup(expression:service => service.Contains("cms")).Returns(value:true);

        bool actual = cache.Contains(scope:"cms");

        actual.Should().BeTrue();
        serviceMock.Verify(expression:service => service.Contains(scope:"cms"), times:Times.Once);
        serviceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldDelegateClear()
    {
        serviceMock.Setup(expression:service => service.Clear(scope:"cms"));

        cache.Clear(scope:"cms");

        serviceMock.Verify(expression:service => service.Clear(scope:"cms"), times:Times.Once);
        serviceMock.VerifyNoOtherCalls();
    }
}