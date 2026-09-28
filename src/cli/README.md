# skyfs

`skyfs` is a command-line tool to list, copy, move, and delete files on:

- the local disk
- Azure Blob Storage
- AWS S3
- Google Cloud Storage (GCS)

You can copy in every direction: from local to a cloud, from a cloud to local, and from one cloud to a different cloud. All the providers that you configure are active at the same time, so one command can use two clouds.

`skyfs` uses the same [StorageLibrary](../StorageLibrary/) as the Azure Storage Explorer web app.

- [Build and run](#build-and-run)
- [Configuration](#configuration)
- [Locations](#locations)
- [Commands](#commands)
- [Rules](#rules)
- [Examples](#examples)
- [Limits](#limits)
- [Tests](#tests)

## Build and run

You need the [.NET 10.0 SDK](https://dotnet.microsoft.com/en-us/download). The [justfile](../../justfile) at the root of the repo has recipes for the CLI.

Run `skyfs` from the source. The arguments after `just cli` go to `skyfs`:

```sh
just cli providers
just cli ls az://mycontainer/
```

Build a single binary to `./bin/skyfs/skyfs`, and put it on your `PATH`:

```sh
just cli-publish
./bin/skyfs/skyfs --help
```

You can also use `dotnet` directly:

```sh
dotnet build ./src/cli/cli.csproj --configuration Release
dotnet ./src/cli/bin/Release/net10.0/skyfs.dll --help
```

## Configuration

`skyfs` reads its credentials from environment variables. These are the same variables as the web app. For AWS and Google, the standard SDK names also work.

| Provider | Scheme | Variables |
|---|---|---|
| Azure | `az://` | `AZURE_STORAGE_CONNECTIONSTRING`, **or** `AZURE_STORAGE_ACCOUNT` and `AZURE_STORAGE_KEY`. Optional: `AZURE_STORAGE_ENDPOINT` (default `core.windows.net`), and `AZURITE=true` for the [Azurite](https://github.com/Azure/Azurite) emulator. |
| AWS | `s3://` | `AWS_ACCESS_KEY` (or `AWS_ACCESS_KEY_ID`), `AWS_SECRET_KEY` (or `AWS_SECRET_ACCESS_KEY`), and `AWS_REGION` (or `AWS_DEFAULT_REGION`) |
| GCP | `gs://` | `GCP_CREDENTIALS_FILE` (or `GOOGLE_APPLICATION_CREDENTIALS`): the full path to a service account credentials file |

When you set `AZURE_STORAGE_CONNECTIONSTRING`, `skyfs` ignores the other Azure variables.

`skyfs` does not use `CLOUD_PROVIDER`. It makes each provider available when that provider's variables are set. A provider's client is created only when a command uses it, so a bad setting for one provider does not stop commands that use the other providers.

To see which providers are configured:

```sh
$ skyfs providers
az://  Azure  configured (connection string)
s3://  AWS    configured (region us-east-1)
gs://  GCP    not configured (set GCP_CREDENTIALS_FILE)
```

## Locations

Every command takes one or two locations:

| Location | Meaning |
|---|---|
| `az://<container>/<path>` | A blob or a folder in an Azure container |
| `s3://<bucket>/<path>` | An object or a folder in an S3 bucket |
| `gs://<bucket>/<path>` | An object or a folder in a GCS bucket |
| Any other text | A local path, relative or absolute |

A cloud location without a path, for example `az://mycontainer`, is the root of that container. A location that ends with `/` is a folder.

## Commands

```
skyfs [command] [options]

Commands:
  ls <location>              List containers, folders, and files
  cp <source> <destination>  Copy files in any direction: local, Azure, AWS, and GCP
  mv <source> <destination>  Move files: copy them, then delete each source file after its copy succeeds
  rm <location>              Delete files, or a folder with --recursive
  providers                  Show which cloud providers are configured
```

Each command shows its help with `--help`, for example `skyfs cp --help`.

### ls

```
skyfs ls [<location>] [options]

  -r, --recursive  List all files below the folder, not only one level
```

| Location | Result |
|---|---|
| empty | The containers of every configured provider |
| `az://`, `s3://`, or `gs://` | The containers of that provider |
| A folder | The folders and files in it. With `-r`, all the files below it. |
| A file | Only that file |

Folders show as `<DIR>` and end with `/`. Names are relative to the listed folder:

```
$ skyfs ls az://photos/
     <DIR>  2024/
    1.2 MB  cover.jpg
```

### cp

```
skyfs cp <source> <destination> [options]

  -r, --recursive  Copy the contents of a folder, with all its subfolders
  -f, --force      Overwrite files that already exist at the destination
```

### mv

```
skyfs mv <source> <destination> [options]

  -r, --recursive  Copy the contents of a folder, with all its subfolders
  -f, --force      Overwrite files that already exist at the destination
```

`mv` copies each file, then deletes the source file. It deletes a source file only after its copy succeeds.

### rm

```
skyfs rm <location> [options]

  -r, --recursive  Delete a folder and all its contents
  -y, --yes        Do not ask before a recursive delete
```

### providers

```
skyfs providers
```

Shows each provider, and if it is configured. When a provider is not configured, it shows the variables to set.

## Rules

**A file into a folder.** When the destination ends with `/`, or is a folder that exists, the file goes into it with its original name. Any other destination is the new name of the file.

```sh
skyfs cp ./a.jpg s3://bucket/images/     # → s3://bucket/images/a.jpg
skyfs cp ./a.jpg s3://bucket/b.jpg       # → s3://bucket/b.jpg
```

**Folders need `-r`.** `cp -r` and `mv -r` copy the **contents** of the source folder into the destination folder. This is the same as `aws s3 cp --recursive`:

```sh
skyfs cp -r az://box/photos ./backup     # → ./backup/a.jpg, ./backup/2024/b.jpg
```

**No overwrite without `--force`.** When a file exists at the destination, `cp` and `mv` stop for that file and show an error. `skyfs` checks before the download, so it does not download a large file for nothing. The upload also refuses to overwrite, and the provider does that check in the same request, so a different writer cannot cause a race.

**A recursive copy continues after an error.** Each file that fails shows an error. The other files are still copied. At the end, `skyfs` shows a summary, for example `41 copied, 1 failed`.

**A move keeps the source when the copy fails.** When `mv -r` moves a local folder, it removes the folders that become empty.

**`rm -r` asks first.** It shows the number of files and asks you to confirm. Use `--yes` to skip the question, for example in a script. When the input is not a terminal and you do not give `--yes`, `rm -r` deletes nothing.

**The same file cannot be the source and the destination.** `skyfs` refuses that copy or move.

**Exit codes.** `0` when all the work succeeded. `1` when anything failed.

**Signatures are hidden.** When an error message contains a SAS URL, `skyfs` replaces the signature with `REDACTED`.

## Examples

```sh
# What can I reach?
skyfs providers
skyfs ls

# List
skyfs ls s3://bucket/                        # one level
skyfs ls gs://bucket/logs/ -r                # all files below logs/
skyfs ls ./downloads

# Local → cloud
skyfs cp ./report.pdf az://docs/2024/
skyfs cp -r ./site s3://my-site-bucket/

# Cloud → local
skyfs cp gs://bucket/data.csv ./
skyfs cp -r az://backups/db/ ./restore

# Cloud → cloud
skyfs cp az://box/a.txt s3://bucket/a.txt
skyfs cp -r s3://bucket/images/ gs://bucket/images/

# Overwrite
skyfs cp -f ./report.pdf az://docs/2024/report.pdf

# Move (cut)
skyfs mv ./export.zip s3://bucket/exports/
skyfs mv -r az://old/ az://new/

# Delete
skyfs rm gs://bucket/tmp.txt
skyfs rm -r s3://bucket/tmp/                 # asks first
skyfs rm -r -y s3://bucket/tmp/              # does not ask
```

## Limits

- **Every copy goes through your machine.** `skyfs` downloads the file to a temp file, uploads it, and then deletes the temp file. This is also true for a copy inside one provider, so a large cloud-to-cloud copy uses your network.
- **S3 uploads larger than 5 GB fail.** The library uploads to S3 with one request, not with a multipart upload.
- **AWS uses one region**, `AWS_REGION`. A bucket in a different region can fail.
- **`rm -r` on S3 can leave empty folder placeholders.** These are zero-byte `folder/` objects, for example from the S3 console. `skyfs ls` does not show them.
- **Only blob containers and buckets.** Azure File Shares, Queues, and Tables are not available in `skyfs`.

## Tests

The tests use an in-memory fake of the storage interface, so you do not need a cloud account:

```sh
just cli-test
```
