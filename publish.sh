cd "${0%/*}" || exit 1

dotnet publish --configuration Release -o ./bin ./src/web/web.csproj