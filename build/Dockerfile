FROM mcr.microsoft.com/dotnet/sdk:11.0 AS build

WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out src/MiniAuth.Host

FROM mcr.microsoft.com/dotnet/aspnet:11.0

WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "MiniAuth.Host.dll"]
