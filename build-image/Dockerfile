FROM mcr.microsoft.com/dotnet/sdk:6.0-focal

LABEL maintainer="seba gomez <@sebagomez>"

ENV SKIP_SASS_BINARY_DOWNLOAD_FOR_CI=true SKIP_NODE_SASS_TESTS=true

RUN curl -sL https://deb.nodesource.com/setup_14.x | bash - && \
    apt update && \
    apt-get install -y nodejs && \
    apt-get install -y build-essential && \
    apt-get install -y python2.7 && \
    npm config set python /usr/bin/python2.7 && \
    npm install -g @angular/cli 

EXPOSE 80 443 5000 5001
