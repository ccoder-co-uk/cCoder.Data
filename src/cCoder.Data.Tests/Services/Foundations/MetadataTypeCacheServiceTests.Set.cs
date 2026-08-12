// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Exceptions;
using cCoder.Data.Brokers.Caching;
using cCoder.Data.Services.Foundations;
using FluentAssertions;
using Xunit;

namespace cCoder.Data.Tests.Services.Foundations;

public sealed partial class MetadataTypeCacheServiceTests
{
    [Fact]
    public void ShouldWrapInvalidOperationExceptionOnSet()
    {
        // Given
        InvalidOperationException dependencyException = new();

        MetadataTypeCacheService service =
            CreateMetadataTypeCacheService(
                broker: new ExceptionMetadataTypeCacheBroker(
                    exception: dependencyException));

        // When
        Action setAction = () => service.Set(
            scope: "cms",
            typeSetPayloads: CreateTypeSetPayloads(names: ["App"]));

        // Then
        setAction
            .Should()
            .Throw<ServiceDependencyException>()
            .WithInnerException<InvalidOperationException>();
    }

    [Fact]
    public void ShouldWrapUnexpectedExceptionOnSet()
    {
        // Given
        Exception unexpectedException = new();

        MetadataTypeCacheService service =
            CreateMetadataTypeCacheService(
                broker: new ExceptionMetadataTypeCacheBroker(
                    exception: unexpectedException));

        // When
        Action setAction = () => service.Set(
            scope: "cms",
            typeSetPayloads: CreateTypeSetPayloads(names: ["App"]));

        // Then
        setAction
            .Should()
            .Throw<ServiceException>()
            .WithInnerException<Exception>();
    }

    private sealed class ExceptionMetadataTypeCacheBroker(Exception exception)
        : IMetadataTypeCacheBroker
    {
        public void Set(string scope, string[] typeSetPayloads) =>
            throw exception;

        public string[] Get(string scope) =>
            throw exception;

        public string[] GetAll() =>
            throw exception;

        public bool Contains(string scope) =>
            throw exception;

        public void Clear(string scope) =>
            throw exception;
    }
}