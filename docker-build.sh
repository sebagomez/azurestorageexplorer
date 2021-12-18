version=$(./version.sh)
docker build -t sebagomez/azurewebstorageexplorer:local-$version -f ./src/Dockerfile ./src/