// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models;
using cCoder.Security.Models;
using FluentAssertions;
using Xunit;

namespace Data.Web.Tests;

public sealed partial class AppConfigurationTests
{
    [Fact]
    public void ShouldExposeEveryRequiredDomainConfiguration()
    {
        // Given
        const string typeName =
            "Data.Web.Models.AppConfiguration, Data.Web";

        // When
        Type configurationType = Type.GetType(typeName: typeName);

        // Then
        configurationType.Should()
            .NotBeNull();

        configurationType.GetProperty(name: "CoreData")
            .PropertyType.Should()
            .Be(expected: typeof(CoreDataConfiguration));

        configurationType.GetProperty(name: "Security")
            .PropertyType.Should()
            .Be(expected: typeof(SecurityConfiguration));

        configurationType.GetProperty(name: "SecurityData")
            .PropertyType.Should()
            .Be(expected: typeof(SecurityDataConfiguration));
    }
}