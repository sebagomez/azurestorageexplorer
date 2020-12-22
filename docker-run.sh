cwd=$(pwd)
docker run --rm -it -p 5000:5000 -p 5001:5001 -p 80:80 -p 443:443 -v $cwd:/code sebagomez/buildazurestorage