FROM sebagomez/buildazurestorage as builder

WORKDIR /src
COPY ./ /src

ENV SKIP_SASS_BINARY_DOWNLOAD_FOR_CI=true SKIP_NODE_SASS_TESTS=true
RUN dotnet publish --configuration Release -o ./bin ./AzureWebStorageExplorer/AzureWebStorageExplorer.csproj

FROM mcr.microsoft.com/dotnet/aspnet:5.0

LABEL maintainer="seba gomez <@sebagomez>"

ARG BUILD
ENV APPVERSION=$BUILD

WORKDIR /app

COPY --from=builder /src/bin ./

ENTRYPOINT ["dotnet", "AzureWebStorageExplorer.dll"]
