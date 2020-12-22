FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as builder

RUN curl -sL https://deb.nodesource.com/setup_10.x | bash - && \
    apt update && \
    apt-get install -y nodejs && \
    npm install -g @angular/cli

WORKDIR /src

COPY ./ /src

RUN dotnet publish --configuration Release -o ./bin ./AzureWebStorageExplorer/AzureWebStorageExplorer.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

LABEL maintainer="seba gomez <@sebagomez>"

ARG BUILD
ENV APPVERSION=$BUILD

WORKDIR /app

COPY --from=builder /src/bin ./

ENTRYPOINT ["dotnet", "AzureWebStorageExplorer.dll"]
