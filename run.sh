cwd=$(pwd)
conf=Debug
docker run --rm -it -P -v $cwd:/code -w /code sebagomez/buildazurestorage dotnet run --configuration $conf --name azure_explorer --project ./src/AzureWebStorageExplorer/AzureWebStorageExplorer.csproj
