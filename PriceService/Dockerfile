FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY PriceService.sln ./
COPY PriceService.API/PriceService.API.csproj PriceService.API/
COPY PriceService.Contracts/PriceService.Contracts.csproj PriceService.Contracts/
COPY PriceService.Infrastructure/PriceService.Infrastructure.csproj PriceService.Infrastructure/

RUN dotnet restore PriceService.API/PriceService.API.csproj

COPY . .
WORKDIR /src/PriceService.API
RUN dotnet publish PriceService.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PriceService.API.dll"]