// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;

namespace Data.Web.AcceptanceTests.Infrastructure;

internal sealed class WebAcceptanceFactory : WebApplicationFactory<Program>
{
    private readonly AcceptanceConnectionStrings connectionStrings;

    public WebAcceptanceFactory() =>
        connectionStrings =
            AcceptanceTestConfiguration.Current
                .CreateConnectionStrings();

    protected override void ConfigureWebHost(
        IWebHostBuilder builder) =>
        builder.UseEnvironment(environment: "Acceptance");

    protected override void Dispose(bool disposing)
    {
        try
        {
            base.Dispose(disposing: disposing);
        }
        finally
        {
            if (disposing)
            {
                DropDatabases();
            }
        }
    }

    private void DropDatabases()
    {
        List<Exception> exceptions = [];

        foreach (string connectionString in new[]
        {
            connectionStrings.Data,
            connectionStrings.Security,
        })
        {
            try
            {
                DropDatabase(connectionString: connectionString);
            }
            catch (Exception exception)
            {
                exceptions.Add(item: exception);
            }
        }

        if (exceptions.Count > 0)
        {
            throw new AggregateException(
                message: "One or more acceptance databases could not be removed.",
                innerExceptions: exceptions);
        }
    }

    private static void DropDatabase(string connectionString)
    {
        SqlConnectionStringBuilder builder = new(
            connectionString: connectionString);

        string databaseName = builder.InitialCatalog;

        if (!databaseName.Contains(
                value: "-acceptance-",
                comparisonType: StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                message:
                    $"Refusing to drop non-acceptance database " +
                    $"'{databaseName}'.");
        }

        builder.InitialCatalog = "master";

        using SqlConnection connection = new(
            connectionString: builder.ConnectionString);

        connection.Open();

        using SqlCommand command = connection.CreateCommand();

        command.CommandText = @"
IF DB_ID(@databaseName) IS NOT NULL
BEGIN
    DECLARE @sql nvarchar(max) =
        N'ALTER DATABASE [' + REPLACE(@databaseName, ']', ']]') + N'] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;'
        + N'DROP DATABASE [' + REPLACE(@databaseName, ']', ']]') + N']';
    EXEC(@sql);
END";

        _ = command.Parameters.AddWithValue(
            parameterName: "@databaseName",
            value: databaseName);

        command.ExecuteNonQuery();
    }
}