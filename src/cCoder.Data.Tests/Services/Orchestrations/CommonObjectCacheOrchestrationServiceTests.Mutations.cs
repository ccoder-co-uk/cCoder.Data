// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using cCoder.Data.Models;
using cCoder.Data.Models.Exceptions;
using cCoder.Data.Services.Foundations;
using cCoder.Data.Services.Orchestrations;
using FluentAssertions;
using Moq;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace cCoder.Data.Tests.Services.Orchestrations;

public sealed partial class CommonObjectCacheOrchestrationServiceTests
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
    public async Task UpdateAsync_WhenStorageSucceeds_RefreshesCache()
    {
        // Given

        CommonObject commonObject = CreateCommonObject(id: 23);
        Mock<ICommonObjectStorageService> storageServiceMock = new();
        Mock<ICommonObjectCacheService> cacheServiceMock = new();

        storageServiceMock.Setup(expression: service =>
                service.UpdateCommonObjectAsync(
                    updatedCommonObject: commonObject))
            .Returns(value: ValueTask.FromResult(result: commonObject));

        storageServiceMock.Setup(expression: service =>
                service.GetCommonObjectsSnapshot())
            .Returns(value: new[] { commonObject });

        CommonObjectCacheOrchestrationService service = new(
            commonObjectStorageService: storageServiceMock.Object,
            commonObjectCacheService: cacheServiceMock.Object);

        // When

        CommonObject result = await service.UpdateCommonObjectAsync(
            updatedCommonObject: commonObject);

        // Then

        result
            .Should()
            .BeSameAs(expected: commonObject);

        cacheServiceMock.Verify(expression: cacheService =>
            cacheService.SetCommonObjects(
                commonObjects: It.IsAny<CommonObject[]>()),
            times: Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenStorageSucceeds_RefreshesCache()
    {
        // Given

        CommonObject commonObject = CreateCommonObject(id: 29);
        Mock<ICommonObjectStorageService> storageServiceMock = new();
        Mock<ICommonObjectCacheService> cacheServiceMock = new();

        storageServiceMock.Setup(expression: service =>
                service.DeleteCommonObjectAsync(
                    deletedCommonObject: commonObject))
            .Returns(value: ValueTask.FromResult(result: 1));

        storageServiceMock.Setup(expression: service =>
                service.GetCommonObjectsSnapshot())
            .Returns(value: []);

        CommonObjectCacheOrchestrationService service = new(
            commonObjectStorageService: storageServiceMock.Object,
            commonObjectCacheService: cacheServiceMock.Object);

        // When

        int result = await service.DeleteCommonObjectAsync(
            deletedCommonObject: commonObject);

        // Then

        result
            .Should()
            .Be(expected: 1);

        cacheServiceMock.Verify(expression: cacheService =>
            cacheService.SetCommonObjects(
                commonObjects: It.IsAny<CommonObject[]>()),
            times: Times.Once);
    }

    [Fact]
    public async Task DeleteAllAsync_WhenStorageSucceeds_DeletesEachAndRefreshesOnce()
    {
        // Given

        CommonObject[] commonObjects =
        [
            CreateCommonObject(id: 31),
            CreateCommonObject(id: 37)
        ];

        Mock<ICommonObjectStorageService> storageServiceMock = new();
        Mock<ICommonObjectCacheService> cacheServiceMock = new();

        storageServiceMock.Setup(expression: service =>
                service.DeleteCommonObjectAsync(
                    deletedCommonObject: It.IsAny<CommonObject>()))
            .Returns(value: ValueTask.FromResult(result: 1));

        storageServiceMock.Setup(expression: service =>
                service.GetCommonObjectsSnapshot())
            .Returns(value: []);

        CommonObjectCacheOrchestrationService service = new(
            commonObjectStorageService: storageServiceMock.Object,
            commonObjectCacheService: cacheServiceMock.Object);

        // When

        await service.DeleteAllCommonObjectsAsync(
            deletedCommonObjects: commonObjects);

        // Then

        storageServiceMock.Verify(expression: storageService =>
            storageService.DeleteCommonObjectAsync(
                deletedCommonObject: It.IsAny<CommonObject>()),
            times: Times.Exactly(callCount: 2));

        cacheServiceMock.Verify(expression: cacheService =>
            cacheService.SetCommonObjects(
                commonObjects: It.IsAny<CommonObject[]>()),
            times: Times.Once);
    }

    [Fact]
    public void Refresh_WhenCalled_StoresDatabaseSnapshot()
    {
        // Given

        CommonObject[] commonObjects = [CreateCommonObject(id: 41)];
        Mock<ICommonObjectStorageService> storageServiceMock = new();
        Mock<ICommonObjectCacheService> cacheServiceMock = new();

        storageServiceMock.Setup(expression: service =>
                service.GetCommonObjectsSnapshot())
            .Returns(value: commonObjects);

        CommonObjectCacheOrchestrationService service = new(
            commonObjectStorageService: storageServiceMock.Object,
            commonObjectCacheService: cacheServiceMock.Object);

        // When

        service.RefreshCommonObjects();

        // Then

        cacheServiceMock.Verify(expression: cacheService =>
            cacheService.SetCommonObjects(
                commonObjects: commonObjects),
            times: Times.Once);
    }

    [Theory]
    [MemberData(nameof(ExceptionMappings))]
    public void Get_WhenStorageFails_WrapsException(
        Exception serviceException,
        Type expectedExceptionType)
    {
        // Given

        Mock<ICommonObjectStorageService> storageServiceMock = new();

        storageServiceMock.Setup(expression: service =>
                service.GetCommonObjects(ignoreFilters: false))
            .Throws(exception: serviceException);

        CommonObjectCacheOrchestrationService service = new(
            commonObjectStorageService: storageServiceMock.Object,
            commonObjectCacheService: Mock.Of<ICommonObjectCacheService>());

        // When

        Action action = () => service.GetCommonObjects(
            fromCache: false);

        // Then

        Exception actualException = Record.Exception(testCode: action);

        actualException.GetType()
            .Should()
            .Be(expected: expectedExceptionType);
    }

    [Theory]
    [MemberData(nameof(ExceptionMappings))]
    public async Task AddAsync_WhenStorageFails_WrapsException(
        Exception serviceException,
        Type expectedExceptionType)
    {
        // Given

        Mock<ICommonObjectStorageService> storageServiceMock = new();

        storageServiceMock.Setup(expression: service =>
                service.AddCommonObjectAsync(
                    newCommonObject: It.IsAny<CommonObject>()))
            .Throws(exception: serviceException);

        CommonObjectCacheOrchestrationService service = new(
            commonObjectStorageService: storageServiceMock.Object,
            commonObjectCacheService: Mock.Of<ICommonObjectCacheService>());

        // When

        Exception actualException = await Record.ExceptionAsync(
            testCode: async () => await service.AddCommonObjectAsync(
                newCommonObject: new CommonObject()));

        // Then

        actualException.GetType()
            .Should()
            .Be(expected: expectedExceptionType);
    }

    [Theory]
    [MemberData(nameof(ExceptionMappings))]
    public async Task DeleteAllAsync_WhenStorageFails_WrapsException(
        Exception serviceException,
        Type expectedExceptionType)
    {
        // Given

        Mock<ICommonObjectStorageService> storageServiceMock = new();

        storageServiceMock.Setup(expression: service =>
                service.DeleteCommonObjectAsync(
                    deletedCommonObject: It.IsAny<CommonObject>()))
            .Throws(exception: serviceException);

        CommonObjectCacheOrchestrationService service = new(
            commonObjectStorageService: storageServiceMock.Object,
            commonObjectCacheService: Mock.Of<ICommonObjectCacheService>());

        // When

        Exception actualException = await Record.ExceptionAsync(
            testCode: async () => await service.DeleteAllCommonObjectsAsync(
                deletedCommonObjects: [new CommonObject()]));

        // Then

        actualException.GetType()
            .Should()
            .Be(expected: expectedExceptionType);
    }
}