version=$(./version.sh)
docker run --rm -it -p 80:80 -p 443:443 sebagomez/azurewebstorageexplorer:local-$version
