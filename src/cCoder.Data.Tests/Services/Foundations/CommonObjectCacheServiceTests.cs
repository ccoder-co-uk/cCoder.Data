// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Brokers.Caching;
using cCoder.Data.Models;
using cCoder.Data.Models.Exceptions;
using cCoder.Data.Services.Foundations;
using FluentAssertions;
using Moq;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace cCoder.Data.Tests.Services.Foundations;

public sealed partial class CommonObjectCacheServiceTests
{
    public static TheoryData<Exception, Type> ExceptionMappings =>
        new()
        {
            { new ValidationException(), typeof(ServiceValidationException) },
            { new ArgumentException(), typeof(ServiceValidationException) },
            { new InvalidOperationException(), typeof(ServiceDependencyException) },
            { new Exception(), typeof(ServiceException) }
        };

    [Fact]
    public void GetCommonObjects_WhenCalled_ReturnsBrokerObjects()
    {
        // Given

        CommonObject[] expectedCommonObjects = [new CommonObject()];
        Mock<ICommonObjectCacheBroker> brokerMock = new();

        brokerMock.Setup(expression: broker => broker.GetCommonObjects())
            .Returns(value: expectedCommonObjects);

        CommonObjectCacheService service = new(
            commonObjectCacheBroker: brokerMock.Object);

        // When

        CommonObject[] actualCommonObjects = service.GetCommonObjects();

        // Then

        actualCommonObjects
            .Should()
            .BeSameAs(expected: expectedCommonObjects);
    }

    [Fact]
    public void SetCommonObjects_WhenCalled_PassesObjectsToBroker()
    {
        // Given

        CommonObject[] commonObjects = [new CommonObject()];
        Mock<ICommonObjectCacheBroker> brokerMock = new();

        CommonObjectCacheService service = new(
            commonObjectCacheBroker: brokerMock.Object);

        // When

        service.SetCommonObjects(commonObjects: commonObjects);

        // Then

        brokerMock.Verify(expression: broker => broker.SetCommonObjects(
            commonObjects: commonObjects), times: Times.Once);
    }

    [Theory]
    [MemberData(nameof(ExceptionMappings))]
    public void GetCommonObjects_WhenBrokerFails_WrapsException(
        Exception brokerException,
        Type expectedExceptionType)
    {
        // Given

        Mock<ICommonObjectCacheBroker> brokerMock = new();

        brokerMock.Setup(expression: broker => broker.GetCommonObjects())
            .Throws(exception: brokerException);

        CommonObjectCacheService service = new(
            commonObjectCacheBroker: brokerMock.Object);

        // When

        Action action = () => service.GetCommonObjects();

        // Then

        Exception actualException = Record.Exception(testCode: action);

        actualException.GetType()
            .Should()
            .Be(expected: expectedExceptionType);
    }

    [Theory]
    [MemberData(nameof(ExceptionMappings))]
    public void SetCommonObjects_WhenBrokerFails_WrapsException(
        Exception brokerException,
        Type expectedExceptionType)
    {
        // Given

        Mock<ICommonObjectCacheBroker> brokerMock = new();

        brokerMock.Setup(expression: broker => broker.SetCommonObjects(
                commonObjects: It.IsAny<CommonObject[]>()))
            .Throws(exception: brokerException);

        CommonObjectCacheService service = new(
            commonObjectCacheBroker: brokerMock.Object);

        // When

        Action action = () => service.SetCommonObjects(
            commonObjects: [new CommonObject()]);

        // Then

        Exception actualException = Record.Exception(testCode: action);

        actualException.GetType()
            .Should()
            .Be(expected: expectedExceptionType);
    }

    [Fact]
    public void SetCommonObjects_WhenObjectsAreNull_ThrowsValidationException()
    {
        // Given

        CommonObjectCacheService service = new(
            commonObjectCacheBroker: Mock.Of<ICommonObjectCacheBroker>());

        // When

        Action action = () => service.SetCommonObjects(
            commonObjects: null);

        // Then

        action
            .Should()
            .Throw<ServiceValidationException>();
    }
}