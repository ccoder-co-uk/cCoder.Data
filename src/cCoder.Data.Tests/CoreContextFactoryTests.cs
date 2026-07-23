// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace cCoder.Data.Tests;

public class CoreContextFactoryTests
{
    [Fact]
    public void ShouldCreateCoreDataContext()
    {
        var services = new ServiceCollection();
        services.AddSingleton(implementationInstance:Mock.Of<ICoreAuthInfo>());
        services.AddSingleton(implementationInstance:new Config());
        services.AddSingleton(implementationInstance:Mock.Of<ILogger<CoreDataContext>>());

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        var factory = new CoreContextFactory(serviceProvider);

        CoreDataContext context = factory.CreateCoreContext();

        context.Should().NotBeNull();
    }
}