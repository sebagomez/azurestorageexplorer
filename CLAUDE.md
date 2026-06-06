# Azure Storage Explorer

Web-based frontend for managing Azure Storage resources (Blobs, Tables, Queues, File Shares). Started in 2009 as an ASP.NET WebForms app; now on .NET 10 Blazor Server. Also a personal learning playground — the tech stack evolves intentionally.

## Tech Stack

- **.NET 10.0 / Blazor Server** — `src/web/`
- **StorageLibrary** — `src/StorageLibrary/` — provider-agnostic C# library; Azure, AWS, GCP, and Mock implementations behind shared interfaces
- **Build tool:** `just` — see `justfile` at repo root
- **Tests:** MSTest — `tests/StorageLibTests/`
- **Container:** Docker image built via GitHub Actions; Dockerfile at `src/Dockerfile`
- **Orchestration:** Docker Compose, Kubernetes (`k8s/`), Helm chart (`helm/`)

## Project Layout

```
src/
  StorageLibrary/       # Provider library (no UI dependency)
    Interfaces/         # IContainer, IQueue, ITable, IFile
    Azure/              # Azure SDK implementations
    AWS/                # S3 implementation (beta)
    GCP/                # GCS implementation (beta)
    Mocks/              # In-memory stubs used by tests
    StorageFactory.cs   # Entry point — resolves concrete implementations
    Settings.cs         # Reads azure.data JSON file for test credentials
  web/
    Pages/              # Blazor components (one per resource type)
    Shared/             # Layout and nav components
    Utils/              # Credentials, helper utilities
    Program.cs          # ASP.NET host setup
tests/
  StorageLibTests/      # Integration-style tests; require real credentials via azure.data
justfile                # Build, publish, test, Docker tasks
```

## Common Commands

```sh
just build      # dotnet build src/web/web.csproj
just publish    # Release publish → ./bin, then runs on http://localhost:5000
just test       # dotnet test tests/StorageLibTests/StorageLibTests.csproj
just dbuild     # docker build → azurestorageexplorer:local
just drun       # Run local image on http://localhost:8080
just compose    # docker-compose up (Azurite + Explorer)
just uncompose  # docker-compose down
```

## Architecture Patterns

### StorageFactory

`StorageFactory` is the single entry point for storage operations. Constructed with a `StorageFactoryConfig`, it wires up the correct provider implementations behind `IContainer`, `IQueue`, `ITable`, and `IFile`. AWS and GCP only implement `IContainer` (buckets). Mock implementations are used when `config.Mock = true`.

### Blazor Components

All page components extend `BaseComponent` (`src/web/Pages/BaseComponent.razor.cs`). `BaseComponent.OnInitializedAsync` handles auth: loads `Credentials` from `ProtectedSessionStorage`, redirects to `/login` if missing, and builds a `StorageFactory`. Each page component can assume `AzureStorage` is non-null after initialization.

### Credentials & Auth

Users log in with Account+Key, SAS, or Connection String. Credentials are stored in Blazor's `ProtectedSessionStorage`. Environment variables bypass the login screen:

| Variable | Purpose |
|---|---|
| `AZURE_STORAGE_CONNECTIONSTRING` | Takes precedence; skips all other Azure vars |
| `AZURE_STORAGE_ACCOUNT` | Account name (needs KEY + ENDPOINT too) |
| `AZURE_STORAGE_KEY` | Access key |
| `AZURE_STORAGE_ENDPOINT` | Custom endpoint |
| `AZURITE` | Set `true` to target Azurite emulator |
| `CLOUD_PROVIDER` | `AWS` or `GCP` to switch provider |
| `AWS_ACCESS_KEY` / `AWS_SECRET_KEY` / `AWS_REGION` | AWS credentials |
| `GCP_CREDENTIALS_FILE` | Full path to GCP service account JSON |

`BASEPATH` env var sets an ASP.NET path base (for reverse proxy deployments).

## Testing

Tests live in `tests/StorageLibTests/` and are integration tests — they talk to real storage. Credentials come from an `azure.data` JSON file (same format as `Settings.cs` reads). The `TestClass` attribute is commented out on most test classes by default so they don't run in CI without credentials.

To run tests locally, provide a valid `azure.data` file or set `AZURE_STORAGE_CONNECTIONSTRING`.

## Adding a New Cloud Provider

1. Add a new `CloudProvider` enum value in `StorageFactory.cs`
2. Implement the relevant interfaces (`IContainer` at minimum) under `src/StorageLibrary/<Provider>/`
3. Add a new `case` in `StorageFactory` constructor
4. Add environment variable detection in `src/web/Utils/`
5. Mark as beta — multi-cloud support is not yet fully tested

## Adding a New Storage Feature (Azure)

1. Extend the relevant interface in `src/StorageLibrary/Interfaces/`
2. Implement in `src/StorageLibrary/Azure/`
3. Add a stub in `src/StorageLibrary/Mocks/`
4. Add/update Blazor component in `src/web/Pages/`
5. Test against both Azure Storage and Azurite

## Known Limitations

- Only `BlockBlob` upload is supported (not PageBlobs or AppendBlobs)
- AWS and GCP support is beta — only blob/bucket operations, no queues/tables/files
- Azure Government connections are not fully tested
