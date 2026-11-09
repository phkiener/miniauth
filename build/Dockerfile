FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build


WORKDIR /build
COPY ./ ./

RUN dotnet restore
RUN dotnet build --no-restore --configuration Release
RUN dotnet publish --no-restore --no-build --configuration Release -o /publish ./src/Miniauth.Server

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS run

RUN apt-get update && apt-get install -y libcrypt1
RUN ln -s /lib/x86_64-linux-gnu/libcrypt.so.1 /usr/bin/libcrypt.so.1

WORKDIR /app
COPY --from=SDK /publish ./

ENTRYPOINT ["dotnet", "Miniauth.Server.dll"]
