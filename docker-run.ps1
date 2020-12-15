$currDir = (Get-Location).Path -replace '\\','/'

docker run --rm -p 5000:5000 -v ${currDir}:/app -it sebagomez/buildazurestorage

# run ./container-build.sh