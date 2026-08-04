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

public sealed partial class DataEntitySetServiceTests
{
    [Fact]
    public async Task ShouldReturnEntitySetsForAuthenticatedUser()
    {
        // Given
        DataEntitySet[] expected = [new() { Name = "Customers" }];
        Mock<IDataSetBroker> broker = new();

        broker.Setup(expression: broker => broker.GetCurrentSsoUserId())
            .Returns(value: "user-1");

        broker.Setup(expression: broker => broker.SelectEntitySetsAsync(
                cancellationToken: CancellationToken.None))
            .ReturnsAsync(value: expected);

        DataEntitySetService service = new(dataSetBroker: broker.Object);

        // When
        DataEntitySet[] actual = await service.GetEntitySetsAsync(
            cancellationToken: CancellationToken.None);

        // Then
        actual.Should()
            .BeSameAs(expected: expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Guest")]
    public async Task ShouldRejectUnauthenticatedUser(string userId)
    {
        // Given
        Mock<IDataSetBroker> broker = new();

        broker.Setup(expression: broker => broker.GetCurrentSsoUserId())
            .Returns(value: userId);

        DataEntitySetService service = new(dataSetBroker: broker.Object);

        // When
        Func<Task> action = async () => await service.GetEntitySetsAsync(
            cancellationToken: CancellationToken.None);

        // Then
        ServiceException exception = (await action.Should()
            .ThrowAsync<ServiceException>()).Which;

        exception.InnerException.Should()
            .BeOfType<UnauthorizedAccessException>();

        broker.Verify(
            expression: broker => broker.SelectEntitySetsAsync(
                cancellationToken: It.IsAny<CancellationToken>()),
            times: Times.Never);
    }

    [Fact]
    public async Task ShouldTranslateBrokerFailureToDependencyException()
    {
        // Given
        Mock<IDataSetBroker> broker = new();

        broker.Setup(expression: broker => broker.GetCurrentSsoUserId())
            .Returns(value: "user-1");

        broker.Setup(expression: broker => broker.SelectEntitySetsAsync(
                cancellationToken: CancellationToken.None))
            .ThrowsAsync(exception: new InvalidOperationException(
                message: "offline"));

        DataEntitySetService service = new(dataSetBroker: broker.Object);

        // When
        Func<Task> action = async () => await service.GetEntitySetsAsync(
            cancellationToken: CancellationToken.None);

        // Then
        ServiceDependencyException exception =
            (await action.Should()
                .ThrowAsync<ServiceDependencyException>()).Which;

        exception.InnerException.Should()
            .BeOfType<InvalidOperationException>();
    }
}