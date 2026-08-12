// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using cCoder.Data.Models.Exceptions;
using cCoder.Data.Services.Foundations;
using FluentAssertions;
using Xunit;

namespace cCoder.Data.Tests.Services.Foundations;

public sealed partial class MetadataTypeCacheServiceTests
{
    [Fact]
    public void ShouldWrapValidationExceptionOnGet()
    {
        // Given
        ValidationException validationException = new();

        MetadataTypeCacheService service =
            CreateMetadataTypeCacheService(
                broker: new ExceptionMetadataTypeCacheBroker(
                    exception: validationException));

        // When
        Func<string[]> getAction = () => service.Get(scope: "cms");

        // Then
        getAction
            .Should()
            .Throw<ServiceValidationException>()
            .WithInnerException<ValidationException>();
    }

    [Fact]
    public void ShouldWrapArgumentExceptionOnGet()
    {
        // Given
        ArgumentException argumentException = new();

        MetadataTypeCacheService service =
            CreateMetadataTypeCacheService(
                broker: new ExceptionMetadataTypeCacheBroker(
                    exception: argumentException));

        // When
        Func<string[]> getAction = () => service.Get(scope: "cms");

        // Then
        getAction
            .Should()
            .Throw<ServiceValidationException>()
            .WithInnerException<ArgumentException>();
    }

    [Fact]
    public void ShouldWrapInvalidOperationExceptionOnGet()
    {
        // Given
        InvalidOperationException dependencyException = new();

        MetadataTypeCacheService service =
            CreateMetadataTypeCacheService(
                broker: new ExceptionMetadataTypeCacheBroker(
                    exception: dependencyException));

        // When
        Func<string[]> getAction = () => service.Get(scope: "cms");

        // Then
        getAction
            .Should()
            .Throw<ServiceDependencyException>()
            .WithInnerException<InvalidOperationException>();
    }

    [Fact]
    public void ShouldWrapUnexpectedExceptionOnGet()
    {
        // Given
        Exception unexpectedException = new();

        MetadataTypeCacheService service =
            CreateMetadataTypeCacheService(
                broker: new ExceptionMetadataTypeCacheBroker(
                    exception: unexpectedException));

        // When
        Func<string[]> getAction = () => service.Get(scope: "cms");

        // Then
        getAction
            .Should()
            .Throw<ServiceException>()
            .WithInnerException<Exception>();
    }
}