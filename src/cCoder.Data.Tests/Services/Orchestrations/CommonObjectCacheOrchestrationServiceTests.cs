// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models;
using cCoder.Data.Services.Orchestrations;
using cCoder.Data.Services.Foundations;
using FluentAssertions;
using Xunit;

namespace cCoder.Data.Tests.Services.Orchestrations;

public sealed partial class CommonObjectCacheOrchestrationServiceTests
{
    [Fact]
    public void Get_WhenFromCacheIsTrueAndCacheExists_ReadsOnlyCache()
    {
        // Given

        CommonObject[] cachedCommonObjects = [CreateCommonObject(id: 7)];
        RecordingCommonObjectStorageService storageService = new();

        RecordingCommonObjectCacheService cacheService = new(
            cachedCommonObjects: cachedCommonObjects);

        CommonObjectCacheOrchestrationService service = new(
            commonObjectStorageService: storageService,
            commonObjectCacheService: cacheService);

        // When

        CommonObject[] result = service
            .GetCommonObjects(fromCache: true)
            .ToArray();

        // Then

        result
            .Should()
            .Equal(expected: cachedCommonObjects);

        storageService.GetCalls
            .Should()
            .Be(expected: 0);

        cacheService.GetCalls
            .Should()
            .Be(expected: 1);
    }

    [Fact]
    public void Get_WhenFromCacheIsTrueAndCacheIsMissing_LoadsAndStoresDatabaseState()
    {
        // Given

        CommonObject[] storedCommonObjects = [CreateCommonObject(id: 11)];

        RecordingCommonObjectStorageService storageService = new(
            storedCommonObjects: storedCommonObjects);

        RecordingCommonObjectCacheService cacheService = new();

        CommonObjectCacheOrchestrationService service = new(
            commonObjectStorageService: storageService,
            commonObjectCacheService: cacheService);

        // When

        CommonObject[] result = service
            .GetCommonObjects(fromCache: true)
            .ToArray();

        // Then

        result
            .Should()
            .Equal(expected: storedCommonObjects);

        storageService.SnapshotCalls
            .Should()
            .Be(expected: 1);

        cacheService.SetCalls
            .Should()
            .Be(expected: 1);
    }

    [Fact]
    public void Get_WhenFromCacheIsFalse_ReadsOnlyDatabase()
    {
        // Given

        CommonObject[] storedCommonObjects = [CreateCommonObject(id: 13)];

        RecordingCommonObjectStorageService storageService = new(
            storedCommonObjects: storedCommonObjects);

        RecordingCommonObjectCacheService cacheService = new(
            cachedCommonObjects: [CreateCommonObject(id: 17)]);

        CommonObjectCacheOrchestrationService service = new(
            commonObjectStorageService: storageService,
            commonObjectCacheService: cacheService);

        // When

        CommonObject[] result = service
            .GetCommonObjects(fromCache: false)
            .ToArray();

        // Then

        result
            .Should()
            .Equal(expected: storedCommonObjects);

        storageService.GetCalls
            .Should()
            .Be(expected: 1);

        cacheService.GetCalls
            .Should()
            .Be(expected: 0);
    }

    [Fact]
    public async Task AddAsync_WhenStorageSucceeds_RefreshesCacheFromDatabase()
    {
        // Given

        CommonObject newCommonObject = CreateCommonObject(id: 0);
        CommonObject storedCommonObject = CreateCommonObject(id: 19);

        RecordingCommonObjectStorageService storageService = new(
            storedCommonObjects: [storedCommonObject],
            savedCommonObject: storedCommonObject);

        RecordingCommonObjectCacheService cacheService = new();

        CommonObjectCacheOrchestrationService service = new(
            commonObjectStorageService: storageService,
            commonObjectCacheService: cacheService);

        // When

        CommonObject result = await service.AddCommonObjectAsync(
            newCommonObject: newCommonObject);

        // Then

        result
            .Should()
            .BeSameAs(expected: storedCommonObject);

        storageService.AddCalls
            .Should()
            .Be(expected: 1);

        storageService.SnapshotCalls
            .Should()
            .Be(expected: 1);

        cacheService.SetCalls
            .Should()
            .Be(expected: 1);

        cacheService.LastSet
            .Should()
            .Equal(expected: [storedCommonObject]);
    }

    private static CommonObject CreateCommonObject(int id) =>
        new CommonObject
        {
            Id = id,
            Name = $"Object {id}",
            Type = "ContentManagement/Component",
            Culture = string.Empty,
            Key = string.Empty,
            Json = "{}"
        };

    private sealed class RecordingCommonObjectStorageService(
        CommonObject[] storedCommonObjects = null,
        CommonObject savedCommonObject = null)
        : ICommonObjectStorageService
    {
        private readonly CommonObject[] storedCommonObjects =
            storedCommonObjects ?? [];

        public int GetCalls { get; private set; }

        public int SnapshotCalls { get; private set; }

        public int AddCalls { get; private set; }

        public IQueryable<CommonObject> GetCommonObjects(bool ignoreFilters)
        {
            GetCalls++;
            return storedCommonObjects.AsQueryable();
        }

        public CommonObject[] GetCommonObjectSnapshot()
        {
            SnapshotCalls++;
            return storedCommonObjects;
        }

        public ValueTask<CommonObject> AddCommonObjectAsync(CommonObject newCommonObject)
        {
            AddCalls++;

            return ValueTask.FromResult(
                result: savedCommonObject ?? newCommonObject);
        }

        public ValueTask<CommonObject> UpdateCommonObjectAsync(CommonObject updatedCommonObject) =>
            ValueTask.FromResult(result: updatedCommonObject);

        public ValueTask<int> DeleteCommonObjectAsync(CommonObject deletedCommonObject) =>
            ValueTask.FromResult(result: 1);

    }

    private sealed class RecordingCommonObjectCacheService(
        CommonObject[] cachedCommonObjects = null)
        : ICommonObjectCacheService
    {
        private readonly CommonObject[] cachedCommonObjects =
            cachedCommonObjects;

        public int GetCalls { get; private set; }

        public int SetCalls { get; private set; }

        public CommonObject[] LastSet { get; private set; }

        public CommonObject[] GetCommonObjects()
        {
            GetCalls++;
            return cachedCommonObjects;
        }

        public void SetCommonObjects(CommonObject[] commonObjects)
        {
            SetCalls++;
            LastSet = commonObjects;
        }
    }
}