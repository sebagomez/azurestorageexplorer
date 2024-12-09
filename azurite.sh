#!/bin/bash

CONTAINER_NAME="azurite"
IMAGE_NAME="mcr.microsoft.com/azure-storage/azurite"

Check if the container is already running
if [ "$(docker ps -q -f name=${CONTAINER_NAME})" ]; then
    echo "Container ${CONTAINER_NAME} is already running."
else
    # Check if the container exists but is stopped
    if [ "$(docker ps -aq -f status=exited -f name=${CONTAINER_NAME})" ]; then
        echo "Starting existing container ${CONTAINER_NAME}."
        docker start ${CONTAINER_NAME}
    else
        echo "Running a new container named ${CONTAINER_NAME}."
        docker run -d -p 10000:10000 -p 10001:10001 -p 10002:10002 --name ${CONTAINER_NAME} ${IMAGE_NAME}
    fi
fi
