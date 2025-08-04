FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ProductService.sln ./
COPY ProductService.API/ProductService.API.csproj ProductService.API/
COPY ProductService.Application/ProductService.Application.csproj ProductService.Application/
COPY ProductService.Contracts/ProductService.Contracts.csproj ProductService.Contracts/
COPY ProductService.Domain/ProductService.Domain.csproj ProductService.Domain/
COPY ProductService.Infrastructure/ProductService.Infrastructure.csproj ProductService.Infrastructure/

RUN dotnet restore ProductService.API/ProductService.API.csproj

COPY . .
WORKDIR /src/ProductService.API
RUN dotnet publish ProductService.API.csproj -c Release -o /app/publish

FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProductService.API.dll"]