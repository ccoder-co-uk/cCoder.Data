# cCoder.Data

`cCoder.Data` contains the shared data access layer for the cCoder platform.

## Contents

- `src/cCoder.Data`
  The main library package published to NuGet.
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

The NuGet package produced by this repository is:

- `cCoder.Data`

## Publishing

GitHub Actions is configured to publish the main package using NuGet trusted publishing.

Before the first publish, configure a trusted publishing policy on nuget.org for:

- Repository owner: `ccoder-co-uk`
- Repository: `cCoder.Data`
- Workflow file: `publish.yml`

The workflow also expects a `NUGET_USER` repository secret containing the nuget.org profile name used during trusted publishing login.
