// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Brokers.Loggings;
using Data.Web.Exposures;
using Data.Web.Exposures.Controllers;
using Data.Web.Models.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text.Json;
using Xunit;

namespace Data.Web.AcceptanceTests.Tests.Api;

public sealed partial class DataRowsControllerTests
{
    [Fact]
    public async Task PostRowAsync_WhenValidationFails_ShouldLogAndReturnBadRequest()
    {
        // Given
        Dictionary<string, JsonElement> values = [];
        Mock<IDataRowManager> dataRowManager = new();
        Mock<ILoggingBroker> loggingBroker = new();
        ServiceValidationException exception = new(innerException: new Exception());

        dataRowManager
            .Setup(expression: manager => manager.AddRowAsync(
                entitySet: "Customers",
                newValues: values,
                cancellationToken: CancellationToken.None))
            .Throws(exception: exception);

        DataRowsController controller =
            new(
                dataRowService: dataRowManager.Object,
                loggingBroker: loggingBroker.Object);

        // When
        IActionResult result = await controller.PostRowAsync(
            entitySet: "Customers",
            values: values,
            cancellationToken: CancellationToken.None);

        // Then
        result
            .Should()
            .BeOfType<BadRequestObjectResult>();

        loggingBroker.Verify(expression: broker => broker.LogError(
            exception: exception,
            message: "Controller request failed.",
            args: It.IsAny<object[]>()), times: Times.Once);
    }

    [Fact]
    public async Task PostRowAsync_WhenSuccessful_ShouldReturnCreated()
    {
        // Given
        Dictionary<string, JsonElement> values = [];
        Dictionary<string, object> savedRow = [];
        Mock<IDataRowManager> dataRowManager = new();
        Mock<ILoggingBroker> loggingBroker = new();

        dataRowManager
            .Setup(expression: manager => manager.AddRowAsync(
                entitySet: "Customers",
                newValues: values,
                cancellationToken: CancellationToken.None))
            .ReturnsAsync(value: savedRow);

        DataRowsController controller =
            new(
                dataRowService: dataRowManager.Object,
                loggingBroker: loggingBroker.Object);

        // When
        IActionResult result = await controller.PostRowAsync(
            entitySet: "Customers",
            values: values,
            cancellationToken: CancellationToken.None);

        // Then
        ObjectResult response = result
            .Should()
            .BeOfType<ObjectResult>()
            .Subject;

        response.StatusCode
            .Should()
            .Be(expected: StatusCodes.Status201Created);

        response.Value
            .Should()
            .BeSameAs(expected: savedRow);
    }
}