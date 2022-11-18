cd "${0%/*}" || exit 1

ehoo App will run on http://localhost:8080

docker run --rm -p 8080:8080 --name azurestorageexplorer azurestorageexplorer:local