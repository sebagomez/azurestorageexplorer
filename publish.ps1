$currDir = (Get-Location).Path
Remove-Item $currDir\Docker\root -Force -Recurse
dotnet publish $currDir\src\AzureWebStorageExplorer\AzureWebStorageExplorer.csproj -o $currDir\Docker\root