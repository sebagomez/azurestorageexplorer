# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run Commands

Requires [.NET 10.0 SDK](https://dotnet.microsoft.com/en-us/download) and optionally [just](https://github.com/casey/just).

```bash
just build        # build src/web/web.csproj
just publish      # release build to ./bin, then runs via Kestrel on http://localhost:5000
just test         # run tests/StorageLibTests/StorageLibTests.csproj
just dbuild       # build Docker image as azurestorageexplorer:local
just drun         # run local Docker image at http://localhost:8080
just compose      # docker-compose with Azurite + explorer (pre-authenticated)
```

Run a specific test class:
```bash
dotnet test ./tests/StorageLibTests/StorageLibTests.csproj --filter "ClassName=StorageLibTests.ContainersTests"
```

## Architecture

The solution has two projects and one test project:

- **`src/StorageLibrary/`** — cloud-agnostic storage abstraction library
- **`src/web/`** — Blazor Server app that references StorageLibrary
- **`tests/StorageLibTests/`** — MSTest unit tests for StorageLibrary (uses `azure.data` file for credentials, not environment variables)

### StorageLibrary

The core abstraction is `StorageFactory`, which creates provider-specific implementations based on `StorageFactoryConfig`. Four interfaces define the contract:

| Interface | Azure impl | AWS impl | GCP impl |
|-----------|-----------|----------|----------|
| `IContainer` | `AzureContainer` | `AWSBucket` | `GCPBucket` |
| `IQueue` | `AzureQueue` | — | — |
| `ITable` | `AzureTable` | — | — |
| `IFile` | `AzureFile` | — | — |

Mock implementations live in `Mocks/` and are used when `MOCK=true` env var is set or when `StorageFactory()` is called with no arguments. `Common/` contains provider-neutral wrapper types (`BlobItemWrapper`, `QueueWrapper`, `TableEntityWrapper`, etc.) used as return values across the interface layer.

### Blazor Web App

`BaseComponent` is the base class for all storage pages. On init it loads `Credentials` from `ProtectedSessionStorage`, redirects to `/login` if missing, and builds a `StorageFactory` via `Util.GetStorageFactory()`.

`Login.razor.cs` checks environment variables on init and auto-authenticates if they are present, bypassing the login form.

Credentials are stored in browser session storage (encrypted by ASP.NET's Data Protection) under `wasm_*` keys.

A `BASEPATH` environment variable can set a path prefix for reverse proxy deployments.

Prometheus metrics are exposed via `prometheus-net.AspNetCore` and counters are incremented via `BaseComponent.Increment()`.

## Environment Variables

| Variable | Purpose |
|----------|---------|
| `AZURE_STORAGE_CONNECTIONSTRING` | Azure connection string (takes precedence over account/key/endpoint) |
| `AZURE_STORAGE_ACCOUNT` | Azure account name |
| `AZURE_STORAGE_KEY` | Azure access key |
| `AZURE_STORAGE_ENDPOINT` | Custom endpoint (default: `core.windows.net`) |
| `AZURITE` | Set to `true` when connecting to Azurite emulator |
| `CLOUD_PROVIDER` | `AWS` or `GCP` to switch providers |
| `AWS_ACCESS_KEY` / `AWS_SECRET_KEY` / `AWS_REGION` | AWS credentials |
| `GCP_CREDENTIALS_FILE` | Path to GCP service account JSON file |
| `MOCK` | Set to `true` to use in-memory mock implementations |
| `BASEPATH` | URL path prefix for reverse proxy (e.g. `/explorer`) |
