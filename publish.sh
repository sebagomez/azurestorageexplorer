cd "${0%/*}" || exit 1

dotnet publish --configuration Release -o ./bin ./src/web/web.csproj

OK=$?
if [ $OK -eq 0 ]; then
	cd bin
	dotnet web.dll
	cd ..
fi