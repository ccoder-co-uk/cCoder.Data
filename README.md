# cCoder.Data

## Local configuration

Bind a `DataConfiguration` from the owning domain configuration. For an empty
connection string, define that domain's `__ConnectionString` user- or
machine-level environment variable, restart Visual Studio, and run with F5.

Register the supporting data services with either the bound configuration or a
configuration callback:

```csharp
services.AddData(configuration.Data);
```

`cCoder.Data` contains the shared data access layer for the cCoder platform.

[View the latest code coverage report](https://ccoder-co-uk.github.io/cCoder.Data/)

## Contents

- `src/cCoder.Data`
  The main library package published to NuGet.
- `src/cCoder.Data.Tests`
  Unit tests for the data layer.
- `src/Apps/Data.Web`
  A local tooling app that demonstrates the package by exposing authenticated CRUD over the `CoreDataContext`.
- `src/Apps/Data.Web.AcceptanceTests`
  Acceptance tests for the Data tooling app.

## Data Tooling App

`Data.Web` is a local support tool, not a published production app. It provides a tabbed CRUD view over every entity set exposed by `CoreDataContext`.

The app uses the standard cCoder security login flow because the shared data context applies user-aware query filters.

Required configuration:

- `Data__ConnectionString`
- `Security__ConnectionString`
- `Security__DecryptionKey`

Run locally:

```powershell
dotnet run --project src/Apps/Data.Web/Data.Web.csproj
```

## Build

```powershell
dotnet build src/cCoder.Data.slnx -v minimal
```

## Test

```powershell
dotnet test src/cCoder.Data.slnx -v minimal --no-build
```

## Package

The NuGet package produced by this repository is:

- `cCoder.Data`

## Publishing

GitHub Actions is configured to publish the main package using NuGet trusted publishing.

Before the first publish, configure a trusted publishing policy on nuget.org for:

- Repository owner: `ccoder-co-uk`
- Repository: `cCoder.Data`
- Workflow file: `publish.yml`

The workflow also expects a `NUGET_USER` repository secret containing the nuget.org profile name used during trusted publishing login.
