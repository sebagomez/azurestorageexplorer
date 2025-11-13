# Azure Storage Explorer - Agent Guide

## Project Overview

Azure Storage Explorer is a web-based application for managing Azure Storage resources including blobs, tables, queues, and file shares. The application provides an intuitive interface for developers to interact with Azure Storage without installing local clients.

**Live Demo:** https://azurestorage.azurewebsites.net  
**Docker Hub:** https://hub.docker.com/r/sebagomez/azurestorageexplorer/

### Technology Stack

- **Framework:** .NET 8.0 with Blazor Server
- **Previous Versions:** Originally built with ASP.NET WebForms, migrated through .NET Core 2.1, 2.2, 3.1, 5.0, 6, 7, 8, and Angular (later moved to Blazor to avoid npm dependency issues)
- **Build Tool:** [just](https://github.com/casey/just) with justfile for build automation
- **Container:** Docker images available and automatically built via GitHub Actions
- **Deployment:** Supports Docker, Docker Compose, Kubernetes, and Helm

### Key Features

1. **Blob Storage Management**
   - Create public or private containers
   - Upload BlockBlobs (other blob types not yet supported)
   - Download and delete blobs

2. **Queue Management**
   - Create queues
   - Create and manage messages

3. **File Share Support**
   - Navigate file shares
   - Browse directory structures

4. **Table Storage**
   - Create tables and entities
   - Query entities with OData-style operators
   - Support for typed properties with EDM data types

5. **Multi-Cloud Support (Beta)**
   - Azure Storage (primary)
   - AWS S3 buckets
   - Google Cloud Platform (GCP) buckets
   - Local Azurite emulator support

## Project Structure

```
azurestorageexplorer/
├── justfile                 # Build automation commands
├── docker-compose.yml       # Docker Compose manifest for Azurite + Explorer
├── k8s/                     # Kubernetes manifests
│   ├── deployment.yaml
│   └── service.yaml
└── src/                     # .NET 8.0 Blazor Server application
```

## Authentication & Configuration

### Connection Methods

The application supports multiple authentication methods:

1. **Account Name + Key**
2. **Shared Access Signature (SAS)**
3. **Connection String** (recommended)

### Environment Variables

**Azure Storage:**
- `AZURE_STORAGE_CONNECTIONSTRING` - Full connection string (takes precedence)
- `AZURE_STORAGE_ACCOUNT` - Account name
- `AZURE_STORAGE_KEY` - Access key
- `AZURE_STORAGE_ENDPOINT` - Custom endpoint
- `AZURITE` - Set to `true` for Azurite emulator

**AWS S3:**
- `CLOUD_PROVIDER=AWS`
- `AWS_ACCESS_KEY`
- `AWS_SECRET_KEY`
- `AWS_REGION`

**Google Cloud Platform:**
- `CLOUD_PROVIDER=GCP`
- `GCP_CREDENTIALS_FILE` - Full path to service account credentials file

**Note:** If `AZURE_STORAGE_CONNECTIONSTRING` is set, other Azure variables are ignored. Otherwise, all three (account, key, endpoint) must be present.

## Building and Running

### Prerequisites
- .NET 8.0 SDK: https://dotnet.microsoft.com/en-us/download
- just (optional): https://github.com/casey/just

### Local Development

```bash
# Build the project
just build

# Publish to bin folder
just publish

# Application will start with Kestrel, typically on http://localhost:5000
```

### Docker

```bash
# Run latest version
docker run --rm -it -p 8000:8080 sebagomez/azurestorageexplorer

# Access at http://localhost:8000
```

### Docker Compose (with Azurite)

```bash
# Start Azurite + Storage Explorer
just compose

# Access at http://localhost:8080 (auto-logged into Azurite)
```

### Kubernetes

```bash
# Apply manifests
kubectl apply -f ./k8s

# Port forward
kubectl port-forward svc/azurestorageexplorer 8080:8080

# Access at http://localhost:8080
```

### Helm Chart (v2.7.1+)

```bash
# Add repository
helm repo add sebagomez https://sebagomez.github.io/azurestorageexplorer

# Install chart
helm install azurestorageexplorer sebagomez/azurestorageexplorer

# Port forward
kubectl port-forward service/azurestorageexplorer 8080:8080
```

## Working with Table Storage

### Creating Entities

Entities are created using property-value pairs, one per line in the format:
```
<PropertyName>='<PropertyValue>'
```

**Example: Creating a movie entity**
```
PartitionKey=Action
RowKey=1
Title=Die Hard
```

**Default Values:**
- `PartitionKey`: "1" (if not specified)
- `RowKey`: Current timestamp (if not specified)

### Typed Properties

Set data types using EDM notation:
```
Year=1978
[Year@odata.type]=Edm.Int32
```

**Supported EDM Types:**
- `Edm.Int64`
- `Edm.Int32`
- `Edm.Boolean`
- `Edm.DateTime`
- `Edm.Double`
- `Edm.Guid`
- Default: String (for any other type)

### Querying Entities

Query syntax: `<PropertyName> [operator] <PropertyValue>`

**Supported Operators:**
- `eq` - equals
- `gt` - greater than
- `ge` - greater or equal
- `lt` - less than
- `le` - less or equal
- `ne` - not equal

**Important:** Operators must have spaces before and after them.

**Example:**
```
PartitionKey eq 'Action'
```

**Note:** Empty query retrieves all entities from the table.

**Reference:** [Azure Storage Query Operators](https://docs.microsoft.com/en-us/rest/api/storageservices/querying-tables-and-entities#supported-comparison-operators)

## Development History

- **2009:** Original implementation in C# with ASP.NET WebForms 2.0
- **2017-2021:** Migrated through multiple .NET Core versions (2.1, 2.2, 3.1) and .NET 5.0, 6, 7
- **Angular Era:** Temporary migration to Angular for modern frontend
- **Current:** .NET 8.0 with Blazor Server (moved away from Angular due to npm dependency management issues)
- **2024+:** Added multi-cloud support (AWS, GCP), Helm charts, and enhanced container orchestration

**Original Blog Post:** https://sgomez.blogspot.com/2009/11/mi-first-useful-azure-application.html

## Testing with Azurite

[Azurite](https://github.com/Azure/Azurite) is the official Azure Storage emulator. The project includes Docker Compose configuration for easy local testing with Azurite.

Set `AZURITE=true` environment variable when connecting to Azurite instances (local, Docker, Docker Compose, or Kubernetes).

## Integration Examples

The Azure Storage Explorer container is commonly used alongside Azurite in development environments. Example from .NET Aspire:

```csharp
var storage = builder.AddAzureStorage("storage").RunAsEmulator();
var blobs = storage.AddBlobs("blobs");

if (storage.Resource.IsEmulator) {
    builder.AddContainer("storage-explorer", "sebagomez/azurestorageexplorer")
        .WithHttpEndpoint(targetPort: 8080)
        .WithEnvironment("AZURE_STORAGE_CONNECTIONSTRING", storage.Resource.EmulatorConnectionString);
}
```

## Contributing Guidelines

When contributing to this project:

1. Maintain compatibility with .NET 8.0
2. Follow Blazor Server best practices
3. Test against both Azure Storage and Azurite
4. Update justfile if adding new build tasks
5. Ensure Docker images build successfully
6. Test Kubernetes deployments if modifying manifests
7. Multi-cloud features are in beta - handle errors gracefully

## Common Development Tasks

### Adding Support for New Storage Features

1. Check Azure Storage SDK compatibility
2. Update relevant Blazor components
3. Add UI elements for the feature
4. Test with both Azure Storage and Azurite
5. Update README.md with feature documentation

### Modifying Container Configuration

1. Update Dockerfile if needed
2. Test Docker build locally
3. Verify GitHub Actions workflow builds correctly
4. Update Kubernetes manifests if ports/env vars change
5. Test Helm chart deployment

### Supporting Additional Cloud Providers

Current providers: Azure, AWS (beta), GCP (beta)

1. Add new environment variable detection
2. Implement provider-specific authentication
3. Map provider APIs to existing interfaces
4. Add configuration documentation
5. Mark as beta until thoroughly tested

## Known Limitations

- Only BlockBlobs supported (not PageBlobs or AppendBlobs)
- Multi-cloud support is in beta
- Azure Government connection not fully tested
- Requires environment variables or manual login for authentication

## Useful Resources

- [Azure Storage Documentation](https://learn.microsoft.com/en-us/azure/storage/)
- [Shared Access Signatures (SAS)](https://learn.microsoft.com/en-us/azure/storage/common/storage-sas-overview)
- [Connection Strings](https://learn.microsoft.com/en-us/azure/storage/common/storage-configure-connection-string)
- [Azurite Emulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite)
- [just Command Runner](https://github.com/casey/just)
- [Helm GitHub Pages](https://github.com/int128/helm-github-pages)

## Quick Reference Commands

```bash
# Build
just build

# Publish
just publish

# Docker Compose
just compose

# Docker run
docker run --rm -it -p 8000:8080 sebagomez/azurestorageexplorer

# Kubernetes apply
kubectl apply -f ./k8s
kubectl port-forward svc/azurestorageexplorer 8080:8080

# Helm install
helm repo add sebagomez https://sebagomez.github.io/azurestorageexplorer
helm install azurestorageexplorer sebagomez/azurestorageexplorer
```

## Version Information

- **Current .NET Version:** 8.0
- **Helm Chart Version:** 2.7.1+
- **Container Registry:** Docker Hub (sebagomez/azurestorageexplorer)
- **CI/CD:** GitHub Actions for automated builds