cd "${0%/*}" || exit 1
docker build --tag azurestorageexplorer:local ./src 