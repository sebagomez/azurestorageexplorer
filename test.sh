cd "${0%/*}" || exit 1
dotnet test ./tests/StorageLibTests/StorageLibTests.csproj 
