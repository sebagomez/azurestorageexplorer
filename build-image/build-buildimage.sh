#Build the image for local use and push it so it can be used by the github action
docker build -t sebagomez/buildazurestorage . && docker push sebagomez/buildazurestorage