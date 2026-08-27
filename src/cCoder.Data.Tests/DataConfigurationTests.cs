// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models;
using FluentAssertions;
using Xunit;

namespace cCoder.Data.Tests;

public sealed partial class DataConfigurationTests
{
    [Fact]
    public void CoreDataConfiguration_ShouldOwnRuntimeAndMigrationConnections()
    {
        // Given
        const string runtimeConnection = "runtime";
        const string migrationConnection = "migration";

        // When
        CoreDataConfiguration configuration = new()
        {
            ConnectionString = runtimeConnection,
            AdminConnectionString = migrationConnection
        };

        // Then
        configuration.ConnectionString
            .Should()
            .Be(expected: runtimeConnection);

        configuration.AdminConnectionString
            .Should()
            .Be(expected: migrationConnection);
    }
}