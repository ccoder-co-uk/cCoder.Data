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

        serviceMock.Setup(service => service.Set("cms", typeSetPayloads));

        cache.Set("cms", typeSetPayloads);

        serviceMock.Verify(service => service.Set("cms", typeSetPayloads), Times.Once);
        serviceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldDelegateGet()
    {
        string[] expected = ["{\"Name\":\"App\"}"];

        serviceMock.Setup(service => service.Get("cms")).Returns(expected);

        string[] actual = cache.Get("cms");

        actual.Should().BeSameAs(expected);
        serviceMock.Verify(service => service.Get("cms"), Times.Once);
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

        serviceMock.Setup(service => service.GetAll()).Returns(expected);

        string[] actual = cache.GetAll();

        actual.Should().BeSameAs(expected);
        serviceMock.Verify(service => service.GetAll(), Times.Once);
        serviceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldDelegateContains()
    {
        serviceMock.Setup(service => service.Contains("cms")).Returns(true);

        bool actual = cache.Contains("cms");

        actual.Should().BeTrue();
        serviceMock.Verify(service => service.Contains("cms"), Times.Once);
        serviceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldDelegateClear()
    {
        serviceMock.Setup(service => service.Clear("cms"));

        cache.Clear("cms");

        serviceMock.Verify(service => service.Clear("cms"), Times.Once);
        serviceMock.VerifyNoOtherCalls();
    }
}

