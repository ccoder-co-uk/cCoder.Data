// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models;
using cCoder.Eventing.Models;
using cCoder.Security.Models.Configurations;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace cCoder.Data.Tests;

public sealed partial class ServiceCollectionExtensionsTests
{
    [Fact]
    public void ShouldPreferEventAuthInfoOverSecurityAuthInfo()
    {
        // Given
        ServiceCollection services = [];

        services.AddScoped<IEventAuthInfo>(
            implementationFactory: _ =>
                new EventAuthInfo { SSOUserId = "event-user" });

        services.AddScoped<ISSOAuthInfo>(
            implementationFactory: _ =>
                new SSOAuthInfo { SSOUserId = "authenticated-user" });

        services.AddData(
            configuration: new DataConfiguration());

        using ServiceProvider serviceProvider =
            services.BuildServiceProvider();

        using IServiceScope scope = serviceProvider.CreateScope();

        // When
        ICoreAuthInfo coreAuthInfo = scope.ServiceProvider
            .GetRequiredService<ICoreAuthInfo>();

        // Then
        coreAuthInfo.SSOUserId.Should()
            .Be(expected: "event-user");
    }

    [Fact]
    public void ShouldResolveGuestWhenAuthInfoIsUnavailable()
    {
        // Given
        ServiceCollection services = [];

        services.AddData(
            configuration: new DataConfiguration());

        using ServiceProvider serviceProvider =
            services.BuildServiceProvider();

        using IServiceScope scope = serviceProvider.CreateScope();

        // When
        ICoreAuthInfo coreAuthInfo = scope.ServiceProvider
            .GetRequiredService<ICoreAuthInfo>();

        // Then
        coreAuthInfo.SSOUserId.Should()
            .Be(expected: "Guest");
    }
}