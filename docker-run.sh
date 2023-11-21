cd "${0%/*}" || exit 1

echo App will run on http://localhost:8080

docker run --rm -p 8080:80 --name azurestorageexplorer azurestorageexplorer:local