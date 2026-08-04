// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Brokers;
using Data.Web.Models;
using Data.Web.Models.Exceptions;
using Data.Web.Services.Foundations;
using FluentAssertions;
using Moq;
using Xunit;

namespace Data.Web.Tests.Services.Foundations;

public sealed class DataEntitySetServiceTests
{
    [Fact]
    public async Task ShouldReturnEntitySetsForAuthenticatedUser()
    {
        DataEntitySet[] expected = [new() { Name = "Customers" }];
        Mock<IDataSetBroker> broker = new();
        broker.Setup(expression: broker => broker.GetCurrentSsoUserId()).Returns(value: "user-1");
        broker.Setup(expression: broker => broker.SelectEntitySetsAsync(CancellationToken.None)).ReturnsAsync(value: expected);
        DataEntitySetService service = new(dataSetBroker: broker.Object);

        DataEntitySet[] actual = await service.GetEntitySetsAsync(CancellationToken.None);

        actual.Should().BeSameAs(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Guest")]
    public async Task ShouldRejectUnauthenticatedUser(string userId)
    {
        Mock<IDataSetBroker> broker = new();
        broker.Setup(expression: broker => broker.GetCurrentSsoUserId()).Returns(value: userId);
        DataEntitySetService service = new(dataSetBroker: broker.Object);

        Func<Task> action = async () => await service.GetEntitySetsAsync(CancellationToken.None);

        ServiceException exception = (await action.Should().ThrowAsync<ServiceException>()).Which;
        exception.InnerException.Should().BeOfType<UnauthorizedAccessException>();
        broker.Verify(expression: broker => broker.SelectEntitySetsAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ShouldTranslateBrokerFailureToDependencyException()
    {
        Mock<IDataSetBroker> broker = new();
        broker.Setup(expression: broker => broker.GetCurrentSsoUserId()).Returns(value: "user-1");
        broker.Setup(expression: broker => broker.SelectEntitySetsAsync(CancellationToken.None))
            .ThrowsAsync(exception: new InvalidOperationException("offline"));
        DataEntitySetService service = new(dataSetBroker: broker.Object);

        Func<Task> action = async () => await service.GetEntitySetsAsync(CancellationToken.None);

        ServiceDependencyException exception =
            (await action.Should().ThrowAsync<ServiceDependencyException>()).Which;

        exception.InnerException.Should().BeOfType<InvalidOperationException>();
    }
}
