FROM mcr.microsoft.com/dotnet/sdk:10.0 AS SDK

WORKDIR /build
COPY ./ ./

RUN dotnet restore
RUN dotnet build --no-restore --configuration Release
RUN dotnet publish --no-restore --no-build --configuration Release -o /publish ./src/Miniauth.Server

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS RUN

RUN apt install libxcrypt

WORKDIR /app
COPY --from=SDK /publish ./

ENTRYPOINT ["dotnet", "Miniauth.Server.dll"]
