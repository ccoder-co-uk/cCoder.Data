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

public sealed class DataRowServiceTests
{
    [Fact]
    public async Task ShouldClampPagingAndReturnRows()
    {
        DataRows expected = new() { EntitySet = "Customers", Rows = [] };
        Mock<IDataSetBroker> broker = CreateAuthenticatedBroker();
        broker.Setup(expression: broker => broker.SelectRowsAsync("Customers", 0, 500, CancellationToken.None)).ReturnsAsync(value: expected);
        DataRowService service = new(dataSetBroker: broker.Object);

        DataRows actual = await service.GetRowsAsync("Customers", -10, 999, CancellationToken.None);

        actual.Should().BeSameAs(expected);
    }

    [Fact]
    public async Task ShouldAddUpdateAndDeleteRows()
    {
        Dictionary<string, JsonElement> values = new() { ["Name"] = JsonDocument.Parse("\"Ada\"").RootElement };
        Dictionary<string, object> expected = new() { ["Name"] = "Ada" };
        Mock<IDataSetBroker> broker = CreateAuthenticatedBroker();
        broker.Setup(expression: broker => broker.InsertRowAsync("Customers", values, CancellationToken.None)).ReturnsAsync(value: expected);
        broker.Setup(expression: broker => broker.UpdateRowAsync("Customers", values, CancellationToken.None)).ReturnsAsync(value: expected);
        DataRowService service = new(dataSetBroker: broker.Object);

        (await service.AddRowAsync("Customers", values, CancellationToken.None)).Should().BeSameAs(expected);
        (await service.UpdateRowAsync("Customers", values, CancellationToken.None)).Should().BeSameAs(expected);
        await service.DeleteRowAsync("Customers", values, CancellationToken.None);

        broker.Verify(expression: broker => broker.DeleteRowAsync("Customers", values, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ShouldRejectNullValuesBeforeCallingBroker()
    {
        Mock<IDataSetBroker> broker = CreateAuthenticatedBroker();
        DataRowService service = new(dataSetBroker: broker.Object);

        Func<Task> action = async () => await service.AddRowAsync("Customers", null, CancellationToken.None);

        ServiceValidationException exception =
            (await action.Should().ThrowAsync<ServiceValidationException>()).Which;

        exception.InnerException.Should().BeOfType<ArgumentNullException>();
        broker.Verify(expression: broker => broker.InsertRowAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, JsonElement>>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ShouldRejectGuestBeforeCallingBroker()
    {
        Mock<IDataSetBroker> broker = new();
        broker.Setup(expression: broker => broker.GetCurrentSsoUserId()).Returns(value: "Guest");
        DataRowService service = new(dataSetBroker: broker.Object);

        Func<Task> action = async () => await service.GetRowsAsync("Customers", 0, 10, CancellationToken.None);

        ServiceException exception = (await action.Should().ThrowAsync<ServiceException>()).Which;
        exception.InnerException.Should().BeOfType<UnauthorizedAccessException>();
        broker.Verify(expression: broker => broker.SelectRowsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ShouldTranslateBrokerFailureToDependencyException()
    {
        Mock<IDataSetBroker> broker = CreateAuthenticatedBroker();
        broker.Setup(expression: broker => broker.SelectRowsAsync("Customers", 0, 10, CancellationToken.None))
            .ThrowsAsync(exception: new InvalidOperationException("offline"));
        DataRowService service = new(dataSetBroker: broker.Object);

        Func<Task> action = async () => await service.GetRowsAsync("Customers", 0, 10, CancellationToken.None);

        ServiceDependencyException exception =
            (await action.Should().ThrowAsync<ServiceDependencyException>()).Which;

        exception.InnerException.Should().BeOfType<InvalidOperationException>();
    }

    private static Mock<IDataSetBroker> CreateAuthenticatedBroker()
    {
        Mock<IDataSetBroker> broker = new();
        broker.Setup(expression: broker => broker.GetCurrentSsoUserId()).Returns(value: "user-1");
        return broker;
    }
}
