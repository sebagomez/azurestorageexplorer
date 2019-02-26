FROM microsoft/dotnet:2.2-aspnetcore-runtime

LABEL maintainer="seba gomez <sebagomezcorrea@outlook.com>"

ARG BUILD
ENV APPVERSION=$BUILD

WORKDIR /app

COPY ["root", "/app"]

ENTRYPOINT ["dotnet", "AngularWebStorageExplorer.dll"]
