# run with DOCKER_BUILDKIT=1 docker build . in the running app directory (with Dockerfile in the target dir)
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
# build image app folder
WORKDIR /app 

# Copy everything else and publish (publish will build and restore as necessary)
COPY . ./

ARG DOTNET_BUILD_CONFIG=Release
RUN echo "Publishing in $DOTNET_BUILD_CONFIG mode"
RUN dotnet publish -c $DOTNET_BUILD_CONFIG -o out --source https://package-server.acr.org/repository/triad-nuget-group --source https://package-server.acr.org/repository/acr-connect-nuget-hosted --source https://package-server.acr.org/repository/triad-nuget-hosted
                    
# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
ARG DOTNET_BUILD_CONFIG=Release
RUN if [ "$DOTNET_BUILD_CONFIG" = "Debug" ]; \
    then apt update && \
    apt install unzip procps -y && \
    curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l /vsdbg; \
    else echo "Building in $DOTNET_BUILD_CONFIG mode, skipping install of remote debugging tools."; \
    fi
EXPOSE 80
EXPOSE 443
WORKDIR /app

ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.2.1/wait /wait
RUN chmod +x /wait

COPY --from=build-env /app/src/Acrconnect.Anonymization.Service.API/out .

CMD /wait && dotnet AcrConnect.Anonymization.Service.API.dll