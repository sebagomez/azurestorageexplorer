[![Build status](https://travis-ci.org/sebagomez/azurestorageexplorer.svg?branch=master)](https://travis-ci.org/sebagomez/azurestorageexplorer)
[![Build Status](https://dev.azure.com/sebagomez/azurestorageexplorer/_apis/build/status/sebagomez.azurestorageexplorer)](https://dev.azure.com/sebagomez/azurestorageexplorer/_build/latest?definitionId=3)
[![GitHub Release](https://img.shields.io/azure-devops/release/sebagomez/c6cf6702-2757-4576-8dfa-cf6e44e0b762/3/5.svg?label=GitHub%20Release)](https://dev.azure.com/sebagomez/azurestorageexplorer/_build/latest?definitionId=3)
[![Docker push](https://img.shields.io/azure-devops/release/sebagomez/c6cf6702-2757-4576-8dfa-cf6e44e0b762/2/2.svg?label=Docker%20Push)](https://hub.docker.com/r/sebagomez/azurestorageexplorer)
[![Docker Pulls](https://img.shields.io/docker/pulls/sebagomez/azurestorageexplorer.svg)](https://hub.docker.com/r/sebagomez/azurestorageexplorer)
 
Try it live at https://azurestorage.azurewebsites.net

Or deploy it wherever you want thanks to the newly AzurePipilines created [Docker Images](https://hub.docker.com/r/sebagomez/azurestorageexplorer/)

# Azure Storage Explorer

Azure Storage Web Explorer makes it easier for developers to browse and manage Blobs, Queues and Tables from Azure Storage. You'll no longer have to install a local client to do that. It was originally developed in C# with asp.net and WebForms 2.0, but now it has been migrated to .NET Core ~~2.1~~ 2.2 and Angular.

To login just enter your account name and key or SAS ([Shared Access Signature](https://docs.microsoft.com/en-us/azure/storage/storage-create-storage-account#manage-your-storage-account))

![Screenshot](https://github.com/sebagomez/azurestorageexplorer/blob/master/res/GitMain.png?raw=true)


**Blobs**: Create public or private Containers and Blobs (only BlockBlobs for now). Download or delete your blobs.

**Queues**: Create Queues and messages.

**File Shares**: Navigate across Fil Shares and directories.

**Tables**: Create table and Entities. To create an Entity you'll have to add one property per line in the form of `<PropertyName>=<PropertyValue>`

If you don't set PertitionKey or RowKey default values will be used ("1" for PartitionKey and a current timestamp for RowKey).  
For example to create a new movie:
> PartitionKey=Action  
RowKey=1  
Title=Die Hard  

To query the entities from a table use the following syntax: `<PropertyName> [operator] <ProepertyValue>`
Where the valid operators are:  *eq* (equals), *gt* (greater than), *ge* (greater or equal), *lt* (less than), *le* (less or equal) and *ne* (not equal).   
Take a look at the [supported comparaison operators](https://docs.microsoft.com/en-us/rest/api/storageservices/querying-tables-and-entities#supported-comparison-operators)  
To query action movies use the following:
> PartitionKey eq 'Action'  

*Please note there's a <kbd>space</kbd> character before and after the **eq** operator.*

If you don't write a query the system will retrieve every Entity on the Table

## Docker

This web app is not integrated with Azure Pipelines, and after the build process it'll create a Docker image and publishes it to [hub.docker.com](https://hub.docker.com/r/sebagomez/azurestorageexplorer/).

```Dockerfile
FROM microsoft/dotnet:2.2-aspnetcore-runtime
LABEL maintainer="seba gomez <sebagomezcorrea@outlook.com>"
ARG BUILD
ENV APPVERSION=$BUILD
WORKDIR /app
COPY ["root", "/app"]
ENTRYPOINT ["dotnet", "AngularWebStorageExplorer.dll"]
```

To fire a container with the latest version just run the following command

`docker run --rm -it -p 5555:80 sebagomez/azurestorageexplorer`

Then open your browser and navigate to http://localhost:5555, and voilá!

