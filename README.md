# cCoder.Data

`cCoder.Data` contains the shared data access layer for the cCoder platform.

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

- `ConnectionStrings:Core`
- `ConnectionStrings:SSO`
- `Settings:DecryptionKey`

Run locally:

```powershell
dotnet run --project src/Apps/Data.Web/Data.Web.csproj
```

## Build

```powershell
dotnet build src/cCoder.Data.sln -v minimal
```

## Test

```powershell
dotnet test src/cCoder.Data.sln -v minimal --no-build
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
