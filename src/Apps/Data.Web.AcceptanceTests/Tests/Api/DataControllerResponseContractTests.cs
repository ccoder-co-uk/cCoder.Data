// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Data.Web.Brokers.Loggings;
using Data.Web.Exposures.Controllers;
using Data.Web.Models;
using Data.Web.Models.Exceptions;
using Data.Web.Services.Foundations;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using Xunit;

namespace Data.Web.AcceptanceTests.Tests.Api;

public sealed partial class DataControllerResponseContractTests
{
    private const string SensitiveDetail = "sensitive-injected-detail";

    [Fact]
    public async Task EveryAction_WhenSuccessful_ShouldReturnExpectedResponseAndLogInformation()
    {
        // Given

        foreach (ControllerAction action in CreateActions(exception: null))
        {
            Mock<ILoggingBroker> loggingBroker = new();

            // When

            IActionResult result = await action.InvokeAsync.Invoke(
                arg: loggingBroker.Object);

            // Then

            GetStatusCode(result: result)
                .Should()
                .Be(expected: action.SuccessStatusCode, because: action.Name);

            loggingBroker.Verify(
                expression: broker => broker.LogInformation(
                    message: "Controller request succeeded.",
                    args: It.IsAny<object[]>()),
                times: Times.Once);

            VerifyLogCounts(
                loggingBroker: loggingBroker,
                information: 1,
                warning: 0,
                error: 0);
        }
    }

    [Fact]
    public async Task EveryAction_WhenRejectedOrFailed_ShouldClassifySafelyAndLogExactly()
    {
        // Given

        foreach (FailureOutcome outcome in CreateFailureOutcomes())
        {
            foreach (ControllerAction action in CreateActions(
                exception: outcome.Exception))
            {
                Mock<ILoggingBroker> loggingBroker = new();

                // When

                IActionResult result = await action.InvokeAsync.Invoke(
                    arg: loggingBroker.Object);

                // Then

                GetStatusCode(result: result)
                    .Should()
                    .Be(expected: outcome.StatusCode,
                        because: $"{action.Name} should classify {outcome.Name}");

                string responseBody = GetResponseBody(result: result);

                responseBody
                    .Should()
                    .Be(expected: outcome.SafeResponseBody,
                        because: $"{action.Name} should return a stable safe response");

                responseBody
                    .Should()
                    .NotContain(
                        unexpected: SensitiveDetail,
                        because: "internal exception details must not be exposed");

                VerifyLogCounts(
                    loggingBroker: loggingBroker,
                    information: 0,
                    warning: outcome.WarningCount,
                    error: outcome.ErrorCount);
            }
        }
    }

    private static IEnumerable<ControllerAction> CreateActions(Exception exception)
    {
        IDataRowService rowService = exception is null
            ? new SuccessfulDataRowService()
            : new ThrowingDataRowService(exception: exception);

        IDataEntitySetService entitySetService = exception is null
            ? new SuccessfulDataEntitySetService()
            : new ThrowingDataEntitySetService(exception: exception);

        yield return new ControllerAction(
            Name: "DataRows.GetRowsAsync",
            SuccessStatusCode: StatusCodes.Status200OK,
            InvokeAsync: loggingBroker => new DataRowsController(
                dataRowService: rowService,
                loggingBroker: loggingBroker)
                .GetRowsAsync(
                    entitySet: "Customers",
                    cancellationToken: CancellationToken.None));

        yield return new ControllerAction(
            Name: "DataRows.PostRowAsync",
            SuccessStatusCode: StatusCodes.Status201Created,
            InvokeAsync: loggingBroker => new DataRowsController(
                dataRowService: rowService,
                loggingBroker: loggingBroker)
                .PostRowAsync(
                    entitySet: "Customers",
                    values: [],
                    cancellationToken: CancellationToken.None));

        yield return new ControllerAction(
            Name: "DataRows.PutRowAsync",
            SuccessStatusCode: StatusCodes.Status200OK,
            InvokeAsync: loggingBroker => new DataRowsController(
                dataRowService: rowService,
                loggingBroker: loggingBroker)
                .PutRowAsync(
                    entitySet: "Customers",
                    values: [],
                    cancellationToken: CancellationToken.None));

        yield return new ControllerAction(
            Name: "DataRows.DeleteRowAsync",
            SuccessStatusCode: StatusCodes.Status204NoContent,
            InvokeAsync: loggingBroker => new DataRowsController(
                dataRowService: rowService,
                loggingBroker: loggingBroker)
                .DeleteRowAsync(
                    entitySet: "Customers",
                    values: [],
                    cancellationToken: CancellationToken.None));

        yield return new ControllerAction(
            Name: "DataEntitySets.GetEntitySetsAsync",
            SuccessStatusCode: StatusCodes.Status200OK,
            InvokeAsync: loggingBroker => new DataEntitySetsController(
                dataEntitySetService: entitySetService,
                loggingBroker: loggingBroker)
                .GetEntitySetsAsync(cancellationToken: CancellationToken.None));
    }

    private static IEnumerable<FailureOutcome> CreateFailureOutcomes()
    {
        yield return new FailureOutcome(
            Name: "typed validation",
            Exception: new ServiceValidationException(
                innerException: new ValidationException(message: SensitiveDetail)),
            StatusCode: StatusCodes.Status400BadRequest,
            SafeResponseBody: "The data request is invalid.",
            WarningCount: 1,
            ErrorCount: 0);

        yield return new FailureOutcome(
            Name: "argument validation",
            Exception: new ArgumentException(message: SensitiveDetail),
            StatusCode: StatusCodes.Status400BadRequest,
            SafeResponseBody: "The request is invalid.",
            WarningCount: 1,
            ErrorCount: 0);

        yield return new FailureOutcome(
            Name: "authentication",
            Exception: new AuthenticationException(message: SensitiveDetail),
            StatusCode: StatusCodes.Status401Unauthorized,
            SafeResponseBody: "Authentication failed.",
            WarningCount: 1,
            ErrorCount: 0);

        yield return new FailureOutcome(
            Name: "authorization",
            Exception: new UnauthorizedAccessException(message: SensitiveDetail),
            StatusCode: StatusCodes.Status403Forbidden,
            SafeResponseBody: "Access is denied.",
            WarningCount: 1,
            ErrorCount: 0);

        yield return new FailureOutcome(
            Name: "security",
            Exception: new SecurityException(message: SensitiveDetail),
            StatusCode: StatusCodes.Status403Forbidden,
            SafeResponseBody: "Access is denied.",
            WarningCount: 1,
            ErrorCount: 0);

        yield return new FailureOutcome(
            Name: "typed dependency",
            Exception: new ServiceDependencyException(
                innerException: new InvalidOperationException(
                    message: SensitiveDetail)),
            StatusCode: StatusCodes.Status503ServiceUnavailable,
            SafeResponseBody: "The data service is unavailable.",
            WarningCount: 0,
            ErrorCount: 1);

        yield return new FailureOutcome(
            Name: "typed unexpected",
            Exception: new ServiceException(
                innerException: new Exception(message: SensitiveDetail)),
            StatusCode: StatusCodes.Status500InternalServerError,
            SafeResponseBody: "The data operation failed.",
            WarningCount: 0,
            ErrorCount: 1);

        yield return new FailureOutcome(
            Name: "unexpected",
            Exception: new Exception(message: SensitiveDetail),
            StatusCode: StatusCodes.Status500InternalServerError,
            SafeResponseBody: "The data operation failed.",
            WarningCount: 0,
            ErrorCount: 1);
    }

    private static int GetStatusCode(IActionResult result) =>
        ((IStatusCodeActionResult)result).StatusCode
            ?? StatusCodes.Status200OK;

    private static string GetResponseBody(IActionResult result) =>
        ((ObjectResult)result).Value?.ToString();

    private static void VerifyLogCounts(
        Mock<ILoggingBroker> loggingBroker,
        int information,
        int warning,
        int error)
    {
        loggingBroker.Verify(
            expression: broker => broker.LogInformation(
                message: It.IsAny<string>(),
                args: It.IsAny<object[]>()),
            times: Times.Exactly(callCount: information));

        loggingBroker.Verify(
            expression: broker => broker.LogWarning(
                message: It.IsAny<string>(),
                args: It.IsAny<object[]>()),
            times: Times.Exactly(callCount: warning));

        loggingBroker.Verify(
            expression: broker => broker.LogError(
                exception: It.IsAny<Exception>(),
                message: It.IsAny<string>(),
                args: It.IsAny<object[]>()),
            times: Times.Exactly(callCount: error));
    }

    private sealed record ControllerAction(
        string Name,
        int SuccessStatusCode,
        Func<ILoggingBroker, ValueTask<IActionResult>> InvokeAsync);

    private sealed record FailureOutcome(
        string Name,
        Exception Exception,
        int StatusCode,
        string SafeResponseBody,
        int WarningCount,
        int ErrorCount);

    private sealed class SuccessfulDataRowService : IDataRowService
    {
        public ValueTask<DataRows> GetRowsAsync(
            string entitySet,
            int skip,
            int take,
            CancellationToken cancellationToken) =>
            ValueTask.FromResult(result: new DataRows());

        public ValueTask<Dictionary<string, object>> AddRowAsync(
            string entitySet,
            Dictionary<string, object> newValues,
            CancellationToken cancellationToken) =>
            ValueTask.FromResult(result: newValues);

        public ValueTask<Dictionary<string, object>> UpdateRowAsync(
            string entitySet,
            Dictionary<string, object> updatedValues,
            CancellationToken cancellationToken) =>
            ValueTask.FromResult(result: updatedValues);

        public ValueTask DeleteRowAsync(
            string entitySet,
            Dictionary<string, object> deletedValues,
            CancellationToken cancellationToken) =>
            ValueTask.CompletedTask;
    }

    private sealed class ThrowingDataRowService(Exception exception) : IDataRowService
    {
        public ValueTask<DataRows> GetRowsAsync(
            string entitySet,
            int skip,
            int take,
            CancellationToken cancellationToken) =>
            ValueTask.FromException<DataRows>(exception: exception);

        public ValueTask<Dictionary<string, object>> AddRowAsync(
            string entitySet,
            Dictionary<string, object> newValues,
            CancellationToken cancellationToken) =>
            ValueTask.FromException<Dictionary<string, object>>(exception: exception);

        public ValueTask<Dictionary<string, object>> UpdateRowAsync(
            string entitySet,
            Dictionary<string, object> updatedValues,
            CancellationToken cancellationToken) =>
            ValueTask.FromException<Dictionary<string, object>>(exception: exception);

        public ValueTask DeleteRowAsync(
            string entitySet,
            Dictionary<string, object> deletedValues,
            CancellationToken cancellationToken) =>
            ValueTask.FromException(exception: exception);
    }

    private sealed class SuccessfulDataEntitySetService : IDataEntitySetService
    {
        public ValueTask<DataEntitySet[]> GetEntitySetsAsync(
            CancellationToken cancellationToken) =>
            ValueTask.FromResult(result: Array.Empty<DataEntitySet>());
    }

    private sealed class ThrowingDataEntitySetService(Exception exception) :
        IDataEntitySetService
    {
        public ValueTask<DataEntitySet[]> GetEntitySetsAsync(
            CancellationToken cancellationToken) =>
            ValueTask.FromException<DataEntitySet[]>(exception: exception);
    }
}