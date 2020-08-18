$currDir = (Get-Location).Path -replace '\\','/'

#docker run --rm -v ${currDir}:/app -it node bash

docker run --rm -p 5000:5000 -v ${currDir}:/app -it mcr.microsoft.com/dotnet/core/sdk:3.1

# Go to /app/src/AzureWebStorageExplorer/ClientApp and 'npm audit'