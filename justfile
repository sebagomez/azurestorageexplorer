# This help
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
dbuild:
  #!/bin/bash
  BUILD=$(date +%Y%m%d%H%M%S)
  echo Building azurestorageexplorer:local with BUILD=$BUILD
  docker build --build-arg BUILD=$BUILD --tag azurestorageexplorer:local ./src

# Launches the local docker image (azurestorageexplorer:local) at http://localhost:8080
drun:
  echo App will run on http://localhost:8080
  docker run --rm -p 127.0.0.1:8080:8080 --name azurestorageexplorer azurestorageexplorer:local

dbuildrun:
	just dbuild && just drun

# Extra dotnet test args go after a --, e.g. just dtest -- --filter ClassName~ContainersTests
# Runs the unit tests in a container, so no local dotnet SDK is needed
dtest *ARGS:
  docker build -f ./src/TestDockerfile --tag azurestorageexplorer-tests:local .
  docker run --rm azurestorageexplorer-tests:local {{ARGS}}

# Builds and launches a docker compose with azurestorageexplorer:local and azurite
compose:
  docker-compose -f ./docker-compose/azurestorageexplorer.yaml up --build 

# Stops de docker compose
uncompose:
  docker-compose -f ./docker-compose/azurestorageexplorer.yaml down 

   
# Run the skyfs CLI, e.g. just cli ls az://  (arguments go straight to skyfs)
# UseAppHost=false runs it through the signed dotnet host, which works on machines that block unsigned binaries
cli *ARGS:
  dotnet run --project ./src/SkyFs -p:UseAppHost=false -- {{ARGS}}

# Run the skyfs CLI unit tests (no cloud account needed)
cli-test:
  dotnet test ./tests/SkyFsTests/SkyFsTests.csproj

# Publish skyfs as a single self-contained binary to ./bin/skyfs
cli-publish:
  dotnet publish ./src/SkyFs/SkyFs.csproj --configuration Release -o ./bin/skyfs -p:PublishSingleFile=true --self-contained
