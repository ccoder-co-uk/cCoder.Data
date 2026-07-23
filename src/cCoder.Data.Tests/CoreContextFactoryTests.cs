// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace cCoder.Data.Tests;

public sealed partial class CoreContextFactoryTests
{
    [Fact]
    public void ShouldCreateCoreDataContext()
    {
        // Given
        IServiceProvider serviceProvider = CreateServiceProvider();
        CoreContextFactory factory = CreateCoreContextFactory(serviceProvider: serviceProvider);

        // When
        CoreDataContext context = factory.CreateCoreContext();

        // Then
        context
            .Should()
            .NotBeNull();
    }

    private static CoreContextFactory CreateCoreContextFactory(
        IServiceProvider serviceProvider) =>
        new CoreContextFactory(serviceProvider: serviceProvider);

    private static IServiceProvider CreateServiceProvider()
    {
        ServiceCollection services = [];
        services.AddSingleton(implementationInstance: Mock.Of<ICoreAuthInfo>());
        services.AddSingleton(implementationInstance: new Config());
        services.AddSingleton(implementationInstance: Mock.Of<ILogger<CoreDataContext>>());

        return services.BuildServiceProvider();
    }
}