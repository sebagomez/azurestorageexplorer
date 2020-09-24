cd src/AzureWebStorageExplorer/ClientApp/
npm update
cd ../../../
dotnet publish --configuration Release src/AzureWebStorageExplorer/AzureWebStorageExplorer.csproj -o ./bin
