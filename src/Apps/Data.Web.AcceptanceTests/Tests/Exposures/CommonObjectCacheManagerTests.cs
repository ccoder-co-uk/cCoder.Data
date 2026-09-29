// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cCoder.Data.Exposures;
using cCoder.Data.Models;
using cCoder.Data.Services.Orchestrations;
using FluentAssertions;
using Moq;
using Xunit;

namespace Data.Web.AcceptanceTests.Tests.Exposures;

public sealed partial class CommonObjectCacheManagerTests
{
    [Fact]
    public async Task Operations_WhenCalled_DelegateToOrchestrationService()
    {
        // Given

        CommonObject commonObject = new();

        IQueryable<CommonObject> commonObjects =
            new[] { commonObject }.AsQueryable();

        Mock<ICommonObjectCacheOrchestrationService> serviceMock = new();

        serviceMock.Setup(expression: service => service.GetCommonObjects(
                fromCache: true,
                ignoreFilters: true))
            .Returns(value: commonObjects);

        serviceMock.Setup(expression: service =>
                service.AddCommonObjectAsync(
                    newCommonObject: commonObject))
            .Returns(value: ValueTask.FromResult(result: commonObject));

        serviceMock.Setup(expression: service =>
                service.UpdateCommonObjectAsync(
                    updatedCommonObject: commonObject))
            .Returns(value: ValueTask.FromResult(result: commonObject));

        serviceMock.Setup(expression: service =>
                service.DeleteCommonObjectAsync(
                    deletedCommonObject: commonObject))
            .Returns(value: ValueTask.FromResult(result: 1));

        CommonObjectCacheManager manager = new(
            orchestrationService: serviceMock.Object);

        // When

        IQueryable<CommonObject> getResult = manager.Get(
            fromCache: true,
            ignoreFilters: true);

        CommonObject addResult = await manager.AddAsync(
            newCommonObject: commonObject);

        CommonObject updateResult = await manager.UpdateAsync(
            updatedCommonObject: commonObject);

        int deleteResult = await manager.DeleteAsync(
            deletedCommonObject: commonObject);

        await manager.DeleteAllAsync(
            deletedCommonObjects: [commonObject]);

        manager.Refresh();

        // Then

        getResult
            .Should()
            .BeSameAs(expected: commonObjects);

        addResult
            .Should()
            .BeSameAs(expected: commonObject);

        updateResult
            .Should()
            .BeSameAs(expected: commonObject);

        deleteResult
            .Should()
            .Be(expected: 1);

        serviceMock.Verify(expression: service =>
            service.DeleteAllCommonObjectsAsync(
                deletedCommonObjects: It.IsAny<IEnumerable<CommonObject>>()),
            times: Times.Once);

        serviceMock.Verify(expression: service =>
            service.RefreshCommonObjects(),
            times: Times.Once);
    }
}