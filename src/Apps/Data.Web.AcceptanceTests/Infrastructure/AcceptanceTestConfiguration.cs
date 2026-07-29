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
        string runId = Guid.NewGuid()
            .ToString(format: "N");

        AcceptanceConnectionStrings connectionStrings = new(
            Data: CreateConnectionString(
                connectionString: DataConnectionString,
                runId: runId),
            Security: CreateConnectionString(
                connectionString: SecurityConnectionString,
                runId: runId));

        Environment.SetEnvironmentVariable(
            variable: "Data__ConnectionString",
            value: connectionStrings.Data);

        Environment.SetEnvironmentVariable(
            variable: "Security__ConnectionString",
            value: connectionStrings.Security);

        return connectionStrings;
    }

    private static string GetConnectionString(string variableName)
    {
        string value =
            Environment.GetEnvironmentVariable(variable: variableName)
            ?? Environment.GetEnvironmentVariable(
                variable: variableName,
                target: EnvironmentVariableTarget.User)
            ?? Environment.GetEnvironmentVariable(
                variable: variableName,
                target: EnvironmentVariableTarget.Machine);

        return value
            ?? throw new InvalidOperationException(
                message: $"{variableName} is required.");
    }

    private static string CreateConnectionString(
        string connectionString,
        string runId)
    {
        SqlConnectionStringBuilder builder = new(
            connectionString: connectionString);

        builder.InitialCatalog =
            $"{builder.InitialCatalog}-acceptance-{runId}";

        return builder.ConnectionString;
    }
}

internal sealed record AcceptanceConnectionStrings(
    string Data,
    string Security);