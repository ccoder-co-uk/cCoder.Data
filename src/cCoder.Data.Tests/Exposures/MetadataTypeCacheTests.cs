// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Exposures;
using cCoder.Data.Services.Foundations;
using FluentAssertions;
using Xunit;

namespace cCoder.Data.Tests.Exposures;

public sealed partial class MetadataTypeCacheTests
{
    [Fact]
    public void ShouldDelegateEveryCacheOperationToService()
    {
        // Given

        const string scope = "coverage-scope";
        string[] payloads = ["first", "second"];
        RecordingMetadataTypeCacheService service = new(payloads: payloads);
        MetadataTypeCache exposure = new(service: service);

        // When

        exposure.Set(
            scope: scope,
            typeSetPayloads: payloads);

        string[] scopedPayloads = exposure.Get(scope: scope);
        string[] allPayloads = exposure.GetAll();
        bool containsScope = exposure.Contains(scope: scope);
        exposure.Clear(scope: scope);

        // Then

        scopedPayloads
            .Should()
            .Equal(expected: payloads);

        allPayloads
            .Should()
            .Equal(expected: payloads);

        containsScope
            .Should()
            .BeTrue();

        service.SetCalls
            .Should()
            .Be(expected: 1);

        service.GetCalls
            .Should()
            .Be(expected: 1);

        service.GetAllCalls
            .Should()
            .Be(expected: 1);

        service.ContainsCalls
            .Should()
            .Be(expected: 1);

        service.ClearCalls
            .Should()
            .Be(expected: 1);
    }

    private sealed class RecordingMetadataTypeCacheService(string[] payloads)
        : IMetadataTypeCacheService
    {
        public int SetCalls { get; private set; }

        public int GetCalls { get; private set; }

        public int GetAllCalls { get; private set; }

        public int ContainsCalls { get; private set; }

        public int ClearCalls { get; private set; }

        public void Set(string scope, IEnumerable<string> typeSetPayloads) =>
            SetCalls++;

        public string[] Get(string scope)
        {
            GetCalls++;
            return payloads;
        }

        public string[] GetAll()
        {
            GetAllCalls++;
            return payloads;
        }

        public bool Contains(string scope)
        {
            ContainsCalls++;
            return true;
        }

        public void Clear(string scope) =>
            ClearCalls++;
    }
}