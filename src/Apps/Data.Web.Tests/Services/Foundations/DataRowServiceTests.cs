// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Text.Json;
using Data.Web.Brokers;
using Data.Web.Models;
using Data.Web.Models.Exceptions;
using Data.Web.Services.Foundations;
using FluentAssertions;
using Moq;
using Xunit;

namespace Data.Web.Tests.Services.Foundations;

public sealed partial class DataRowServiceTests
{
    [Fact]
    public async Task ShouldClampPagingAndReturnRows()
    {
        // Given
        DataRows expected = new() { EntitySet = "Customers", Rows = [] };
        Mock<IDataSetBroker> broker = CreateAuthenticatedBroker();

        broker.Setup(expression: broker => broker.SelectRowsAsync(
                entitySet: "Customers",
                skip: 0,
                take: 500,
                cancellationToken: CancellationToken.None))
            .ReturnsAsync(value: expected);

        DataRowService service = new(dataSetBroker: broker.Object);

        // When
        DataRows actual = await service.GetRowsAsync(
            entitySet: "Customers",
            skip: -10,
            take: 999,
            cancellationToken: CancellationToken.None);

        // Then
        actual.Should()
            .BeSameAs(expected: expected);
    }

    [Fact]
    public async Task ShouldAddUpdateAndDeleteRows()
    {
        // Given
        Dictionary<string, JsonElement> values = new()
        {
            ["Name"] = JsonDocument.Parse(json: "\"Ada\"").RootElement
        };

        Dictionary<string, object> expected = new() { ["Name"] = "Ada" };
        Mock<IDataSetBroker> broker = CreateAuthenticatedBroker();

        broker.Setup(expression: broker => broker.InsertRowAsync(
                entitySet: "Customers",
                values: values,
                cancellationToken: CancellationToken.None))
            .ReturnsAsync(value: expected);

        broker.Setup(expression: broker => broker.UpdateRowAsync(
                entitySet: "Customers",
                values: values,
                cancellationToken: CancellationToken.None))
            .ReturnsAsync(value: expected);

        DataRowService service = new(dataSetBroker: broker.Object);

        // When
        Dictionary<string, object> actualAdded = await service.AddRowAsync(
            entitySet: "Customers",
            newValues: values,
            cancellationToken: CancellationToken.None);

        Dictionary<string, object> actualUpdated = await service.UpdateRowAsync(
            entitySet: "Customers",
            updatedValues: values,
            cancellationToken: CancellationToken.None);

        await service.DeleteRowAsync(
            entitySet: "Customers",
            deletedValues: values,
            cancellationToken: CancellationToken.None);

        // Then
        actualAdded.Should()
            .BeSameAs(expected: expected);

        actualUpdated.Should()
            .BeSameAs(expected: expected);

        broker.Verify(
            expression: broker => broker.DeleteRowAsync(
                entitySet: "Customers",
                values: values,
                cancellationToken: CancellationToken.None),
            times: Times.Once);
    }

    [Fact]
    public async Task ShouldRejectNullValuesBeforeCallingBroker()
    {
        // Given
        Mock<IDataSetBroker> broker = CreateAuthenticatedBroker();
        DataRowService service = new(dataSetBroker: broker.Object);

        // When
        Func<Task> action = async () => await service.AddRowAsync(
            entitySet: "Customers",
            newValues: null,
            cancellationToken: CancellationToken.None);

        // Then
        ServiceValidationException exception =
            (await action.Should()
                .ThrowAsync<ServiceValidationException>()).Which;

        exception.InnerException.Should()
            .BeOfType<ArgumentNullException>();

        broker.Verify(
            expression: broker => broker.InsertRowAsync(
                entitySet: It.IsAny<string>(),
                values: It.IsAny<Dictionary<string, JsonElement>>(),
                cancellationToken: It.IsAny<CancellationToken>()),
            times: Times.Never);
    }

    [Fact]
    public async Task ShouldRejectGuestBeforeCallingBroker()
    {
        // Given
        Mock<IDataSetBroker> broker = new();

        broker.Setup(expression: broker => broker.GetCurrentSsoUserId())
            .Returns(value: "Guest");

        DataRowService service = new(dataSetBroker: broker.Object);

        // When
        Func<Task> action = async () => await service.GetRowsAsync(
            entitySet: "Customers",
            skip: 0,
            take: 10,
            cancellationToken: CancellationToken.None);

        // Then
        ServiceException exception = (await action.Should()
            .ThrowAsync<ServiceException>()).Which;

        exception.InnerException.Should()
            .BeOfType<UnauthorizedAccessException>();

        broker.Verify(
            expression: broker => broker.SelectRowsAsync(
                entitySet: It.IsAny<string>(),
                skip: It.IsAny<int>(),
                take: It.IsAny<int>(),
                cancellationToken: It.IsAny<CancellationToken>()),
            times: Times.Never);
    }

    [Fact]
    public async Task ShouldTranslateBrokerFailureToDependencyException()
    {
        // Given
        Mock<IDataSetBroker> broker = CreateAuthenticatedBroker();

        broker.Setup(expression: broker => broker.SelectRowsAsync(
                entitySet: "Customers",
                skip: 0,
                take: 10,
                cancellationToken: CancellationToken.None))
            .ThrowsAsync(exception: new InvalidOperationException(
                message: "offline"));

        DataRowService service = new(dataSetBroker: broker.Object);

        // When
        Func<Task> action = async () => await service.GetRowsAsync(
            entitySet: "Customers",
            skip: 0,
            take: 10,
            cancellationToken: CancellationToken.None);

        // Then
        ServiceDependencyException exception =
            (await action.Should()
                .ThrowAsync<ServiceDependencyException>()).Which;

        exception.InnerException.Should()
            .BeOfType<InvalidOperationException>();
    }

    private static Mock<IDataSetBroker> CreateAuthenticatedBroker()
    {
        Mock<IDataSetBroker> broker = new();

        broker.Setup(expression: broker => broker.GetCurrentSsoUserId())
            .Returns(value: "user-1");

        return broker;
    }
}