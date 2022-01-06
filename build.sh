cwd=$(pwd)
conf=Release
rm -rf ./bin/**
docker run --rm -it -v $cwd:/code -w /code sebagomez/buildazurestorage dotnet publish --configuration $conf ./src/AzureWebStorageExplorer/AzureWebStorageExplorer.csproj -o ./bin
