default:
  @just --list 

# Build the solution
build:
  dotnet build ./src/web/web.csproj

# Publish and launches in localhost:5000
publish:
  #!/bin/bash
  dotnet publish --configuration Release -o ./bin ./src/web/web.csproj

  OK=$?
  if [ $OK -eq 0 ]; then
    echo Azure Storage Explorer will be running in http://localhost:5000/ 
    cd bin
    dotnet web.dll
    cd ..
  fi

# Run unit tests
test:
  dotnet test ./tests/StorageLibTests/StorageLibTests.csproj 

# Build Docker image as azurestorageexplorer:local
db:
  docker build --tag azurestorageexplorer:local ./src

# Launches the local docker image at http://localhost:8080
dr:
  echo App will run on http://localhost:8080
  docker run --rm -p 8080:8080 --name azurestorageexplorer azurestorageexplorer:local

compose:
  docker-compose -f ./docker-compose/azurestorageexplorer.yaml up 

uncompose:
  docker-compose -f ./docker-compose/azurestorageexplorer.yaml down 

   