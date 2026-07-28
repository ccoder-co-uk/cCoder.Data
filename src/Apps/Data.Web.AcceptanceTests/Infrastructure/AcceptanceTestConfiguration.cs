// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Microsoft.Data.SqlClient;

namespace Data.Web.AcceptanceTests.Infrastructure;

internal sealed class AcceptanceTestConfiguration
{
    private AcceptanceTestConfiguration()
    {
        DataConnectionString = GetConnectionString(
            variableName: "Data__ConnectionString");
        SecurityConnectionString = GetConnectionString(
            variableName: "Security__ConnectionString");
    }

    internal static AcceptanceTestConfiguration Current { get; } = new();

    internal string DataConnectionString { get; }

    internal string SecurityConnectionString { get; }

    internal AcceptanceConnectionStrings CreateConnectionStrings()
    {
        AcceptanceConnectionStrings connectionStrings = new(
            Data: CreateConnectionString(
                connectionString: DataConnectionString),
            Security: CreateConnectionString(
                connectionString: SecurityConnectionString));

        Environment.SetEnvironmentVariable(
            variable: "Data__ConnectionString",
            value: connectionStrings.Data);

        Environment.SetEnvironmentVariable(
            variable: "Security__ConnectionString",
            value: connectionStrings.Security);

        return connectionStrings;
    }

    private static string GetConnectionString(string variableName) =>
        Environment.GetEnvironmentVariable(variable: variableName)
            ?? throw new InvalidOperationException(
                message: $"{variableName} is required.");

    private static string CreateConnectionString(string connectionString)
    {
        SqlConnectionStringBuilder builder = new(
            connectionString: connectionString);

        builder.InitialCatalog =
            $"{builder.InitialCatalog}-acceptance-{Guid.NewGuid():N}";

        return builder.ConnectionString;
    }
}

internal sealed record AcceptanceConnectionStrings(
    string Data,
    string Security);