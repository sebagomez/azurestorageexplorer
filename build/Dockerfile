#docker build -t buildazurestorage .
FROM mcr.microsoft.com/dotnet/core/sdk:3.1

RUN curl -sL https://deb.nodesource.com/setup_10.x | bash - && \
    apt update && \
    apt-get install -y nodejs && \
    npm install -g @angular/cli

ENTRYPOINT bash