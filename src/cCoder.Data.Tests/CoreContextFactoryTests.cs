using cCoder.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
        services.AddSingleton(Mock.Of<ICoreAuthInfo>());
        services.AddSingleton(new Config());
        services.AddSingleton(new DbContextOptionsBuilder<CoreDataContext>().Options);
        services.AddSingleton(Mock.Of<ILogger<CoreDataContext>>());

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        var factory = new CoreContextFactory(serviceProvider);

        CoreDataContext context = factory.CreateCoreContext();

        context.Should().NotBeNull();
    }
}
