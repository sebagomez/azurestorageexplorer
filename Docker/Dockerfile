FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

LABEL maintainer="seba gomez <sebagomezcorrea@outlook.com>"

ARG BUILD
ENV APPVERSION=$BUILD

WORKDIR /app

COPY ["root", "/app"]

ENTRYPOINT ["dotnet", "AzureWebStorageExplorer.dll"]
