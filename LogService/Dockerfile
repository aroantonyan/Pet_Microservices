FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY LogService.sln ./
COPY LogService.API/LogService.API.csproj LogService.API/
COPY LogService.Contracts/LogService.Contracts.csproj LogService.Contracts/

RUN dotnet restore LogService.API/LogService.API.csproj

COPY . .

WORKDIR /src/LogService.API
RUN dotnet publish LogService.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "LogService.API.dll"]
