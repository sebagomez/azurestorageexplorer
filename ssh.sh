cwd=$(pwd)
docker run --rm -it -v $cwd:/code -w /code sebagomez/buildazurestorage bash
