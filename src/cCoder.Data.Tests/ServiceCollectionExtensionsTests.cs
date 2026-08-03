// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Security.Models.Configurations;
using cCoder.Data.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace cCoder.Data.Tests;

public sealed partial class ServiceCollectionExtensionsTests
{
    [Fact]
    public void ShouldResolveCoreAuthInfoFromSecurityAuthInfo()
    {
        // Given
        ServiceCollection services = [];

        services.AddScoped<SSOAuthInfo>(
            implementationFactory: _ =>
                new SSOAuthInfo { SSOUserId = "authenticated-user" });

        services.AddScoped<ISSOAuthInfo>(
            implementationFactory: serviceProvider =>
                serviceProvider.GetRequiredService<SSOAuthInfo>());

        services.AddData(
            configuration: new DataConfiguration());

        using ServiceProvider serviceProvider =
            services.BuildServiceProvider();

        using IServiceScope scope = serviceProvider.CreateScope();

        // When
        ICoreAuthInfo authInfo = scope.ServiceProvider
            .GetRequiredService<ICoreAuthInfo>();

        // Then
        authInfo.SSOUserId.Should()
            .Be(expected: "authenticated-user");
    }
}