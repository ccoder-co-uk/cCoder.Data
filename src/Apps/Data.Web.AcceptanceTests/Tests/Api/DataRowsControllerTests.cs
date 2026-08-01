// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Exposures;
using Data.Web.Exposures.Controllers;
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
    public async Task PostRowAsync_WhenSuccessful_ShouldReturnCreated()
    {
        // Given
        Dictionary<string, JsonElement> values = [];
        Dictionary<string, object> savedRow = [];
        Mock<IDataRowManager> dataRowManager = new();

        dataRowManager
            .Setup(expression: manager => manager.AddRowAsync(
                entitySet: "Customers",
                newValues: values,
                cancellationToken: CancellationToken.None))
            .ReturnsAsync(value: savedRow);

        DataRowsController controller =
            new(dataRowService: dataRowManager.Object);

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