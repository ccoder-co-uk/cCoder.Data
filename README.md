# cCoder.Data

`cCoder.Data` contains the shared data access layer for the cCoder platform.

This repository now separates provider-neutral functionality from provider-specific configuration:

- `cCoder.Data` contains shared entities, services, exposures, and common DI registration.
- `cCoder.SQLServer` contains SQL Server-specific EF Core registration and migrations.
- `cCoder.PostgresSQL` contains PostgreSQL-specific registration hooks.

## Contents

- `src/cCoder.Data`
  Shared provider-neutral library package published to NuGet.
- `src/cCoder.SQLServer`
  SQL Server provider package.
- `src/cCoder.PostgresSQL`
  PostgreSQL provider package.
- `src/cCoder.Data.Tests`
  Unit tests for the data layer.

## Build

```powershell
dotnet build src/cCoder.Data.sln -v minimal
```

## Test

```powershell
dotnet test src/cCoder.Data.sln -v minimal --no-build
```

## Package

The NuGet packages produced by this repository are:

- `cCoder.Data`
- `cCoder.SQLServer`
- `cCoder.PostgresSQL`

## Provider Usage

Register shared + provider-specific components in your host application.

SQL Server:

```csharp
using cCoder.SQLServer;

services.AddCoreDataSqlServer(connectionString);
```

PostgreSQL:

```csharp
using cCoder.PostgresSQL;

services.AddCoreDataPostgresSQL(connectionString);
```

## Publishing

GitHub Actions is configured to publish the main package using NuGet trusted publishing.

Before the first publish, configure a trusted publishing policy on nuget.org for:

- Repository owner: `ccoder-co-uk`
- Repository: `cCoder.Data`
- Workflow file: `publish.yml`

The workflow also expects a `NUGET_USER` repository secret containing the nuget.org profile name used during trusted publishing login.
