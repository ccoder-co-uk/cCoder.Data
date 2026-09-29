// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using cCoder.Data.Brokers.Storages;
using cCoder.Data.Models;
using cCoder.Data.Models.Exceptions;
using cCoder.Data.Services.Foundations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace cCoder.Data.Tests.Services.Foundations;

public sealed partial class CommonObjectStorageServiceTests
{
    public static TheoryData<Exception, Type> ExceptionMappings =>
        new()
        {
            { new ValidationException(), typeof(ServiceValidationException) },
            { new ArgumentException(), typeof(ServiceValidationException) },
            { new DbUpdateConcurrencyException(), typeof(ServiceDependencyException) },
            { new InvalidOperationException(), typeof(ServiceDependencyException) },
            { new Exception(), typeof(ServiceException) }
        };

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void GetCommonObjects_WhenCalled_UsesSelectedDatabaseQuery(
        bool ignoreFilters)
    {
        // Given

        IQueryable<CommonObject> expectedCommonObjects =
            new[] { new CommonObject() }.AsQueryable();

        Mock<ICommonObjectStorageBroker> brokerMock = new(
            behavior: MockBehavior.Strict);

        if (ignoreFilters)
        {
            brokerMock.Setup(expression: broker =>
                    broker.GetAllCommonObjectsIgnoringFilters())
                .Returns(value: expectedCommonObjects);
        }
        else
        {
            brokerMock.Setup(expression: broker =>
                    broker.GetAllCommonObjects())
                .Returns(value: expectedCommonObjects);
        }

        CommonObjectStorageService service = new(
            commonObjectStorageBroker: brokerMock.Object);

        // When

        IQueryable<CommonObject> actualCommonObjects =
            service.GetCommonObjects(ignoreFilters: ignoreFilters);

        // Then

        actualCommonObjects
            .Should()
            .BeSameAs(expected: expectedCommonObjects);
    }

    [Fact]
    public void GetCommonObjectSnapshot_WhenCalled_ReturnsBrokerSnapshot()
    {
        // Given

        CommonObject[] expectedCommonObjects = [new CommonObject()];
        Mock<ICommonObjectStorageBroker> brokerMock = new();

        brokerMock.Setup(expression: broker =>
                broker.GetCommonObjectSnapshot())
            .Returns(value: expectedCommonObjects);

        CommonObjectStorageService service = new(
            commonObjectStorageBroker: brokerMock.Object);

        // When

        CommonObject[] actualCommonObjects =
            service.GetCommonObjectSnapshot();

        // Then

        actualCommonObjects
            .Should()
            .BeSameAs(expected: expectedCommonObjects);
    }

    [Fact]
    public async Task AddCommonObjectAsync_WhenCalled_ReturnsBrokerResult()
    {
        // Given

        CommonObject commonObject = new();
        Mock<ICommonObjectStorageBroker> brokerMock = new();

        brokerMock.Setup(expression: broker => broker.AddCommonObjectAsync(
                newCommonObject: commonObject))
            .Returns(value: ValueTask.FromResult(result: commonObject));

        CommonObjectStorageService service = new(
            commonObjectStorageBroker: brokerMock.Object);

        // When

        CommonObject result = await service.AddCommonObjectAsync(
            newCommonObject: commonObject);

        // Then

        result
            .Should()
            .BeSameAs(expected: commonObject);
    }

    [Fact]
    public async Task UpdateCommonObjectAsync_WhenCalled_ClonesAndReturnsBrokerResult()
    {
        // Given

        CommonObject commonObject = CreateCommonObject();
        CommonObject storedCommonObject = null;
        Mock<ICommonObjectStorageBroker> brokerMock = new();

        brokerMock.Setup(expression: broker => broker.UpdateCommonObjectAsync(
                updatedCommonObject: It.IsAny<CommonObject>()))
            .Returns(valueFunction: (CommonObject value) =>
            {
                storedCommonObject = value;
                return ValueTask.FromResult(result: value);
            });

        CommonObjectStorageService service = new(
            commonObjectStorageBroker: brokerMock.Object);

        // When

        CommonObject result = await service.UpdateCommonObjectAsync(
            updatedCommonObject: commonObject);

        // Then

        result
            .Should()
            .BeSameAs(expected: storedCommonObject);

        storedCommonObject
            .Should()
            .BeEquivalentTo(expectation: commonObject);

        storedCommonObject
            .Should()
            .NotBeSameAs(unexpected: commonObject);
    }

    [Fact]
    public async Task DeleteCommonObjectAsync_WhenCalled_ReturnsBrokerResult()
    {
        // Given

        CommonObject commonObject = new();
        Mock<ICommonObjectStorageBroker> brokerMock = new();

        brokerMock.Setup(expression: broker => broker.DeleteCommonObjectAsync(
                deletedCommonObject: commonObject))
            .Returns(value: ValueTask.FromResult(result: 1));

        CommonObjectStorageService service = new(
            commonObjectStorageBroker: brokerMock.Object);

        // When

        int result = await service.DeleteCommonObjectAsync(
            deletedCommonObject: commonObject);

        // Then

        result
            .Should()
            .Be(expected: 1);
    }

    [Theory]
    [MemberData(nameof(ExceptionMappings))]
    public void GetCommonObjects_WhenBrokerFails_WrapsException(
        Exception brokerException,
        Type expectedExceptionType)
    {
        // Given

        Mock<ICommonObjectStorageBroker> brokerMock = new();

        brokerMock.Setup(expression: broker => broker.GetAllCommonObjects())
            .Throws(exception: brokerException);

        CommonObjectStorageService service = new(
            commonObjectStorageBroker: brokerMock.Object);

        // When

        Action action = () => service.GetCommonObjects(
            ignoreFilters: false);

        // Then

        Exception actualException = Record.Exception(testCode: action);

        actualException.GetType()
            .Should()
            .Be(expected: expectedExceptionType);
    }

    [Theory]
    [MemberData(nameof(ExceptionMappings))]
    public async Task AddCommonObjectAsync_WhenBrokerFails_WrapsException(
        Exception brokerException,
        Type expectedExceptionType)
    {
        // Given

        Mock<ICommonObjectStorageBroker> brokerMock = new();

        brokerMock.Setup(expression: broker => broker.AddCommonObjectAsync(
                newCommonObject: It.IsAny<CommonObject>()))
            .Throws(exception: brokerException);

        CommonObjectStorageService service = new(
            commonObjectStorageBroker: brokerMock.Object);

        // When

        Exception actualException = await Record.ExceptionAsync(
            testCode: async () => await service.AddCommonObjectAsync(
                newCommonObject: new CommonObject()));

        // Then

        actualException.GetType()
            .Should()
            .Be(expected: expectedExceptionType);
    }

    private static CommonObject CreateCommonObject() =>
        new CommonObject
        {
            Id = 1,
            Name = "Object",
            Description = "Description",
            LastUpdated = DateTime.UtcNow,
            LastUpdatedBy = "User",
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "User",
            Version = 2,
            Key = "Key",
            Type = "Type",
            Json = "{}",
            Culture = "en-GB"
        };
}