FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["src/SeguroVeiculos.API/SeguroVeiculos.API.csproj", "src/SeguroVeiculos.API/"]
COPY ["src/SeguroVeiculos.Application/SeguroVeiculos.Application.csproj", "src/SeguroVeiculos.Application/"]
COPY ["src/SeguroVeiculos.Domain/SeguroVeiculos.Domain.csproj", "src/SeguroVeiculos.Domain/"]
COPY ["src/SeguroVeiculos.Infrastructure/SeguroVeiculos.Infrastructure.csproj", "src/SeguroVeiculos.Infrastructure/"]

RUN dotnet restore "src/SeguroVeiculos.API/SeguroVeiculos.API.csproj"

COPY . .

RUN dotnet build "src/SeguroVeiculos.API/SeguroVeiculos.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/SeguroVeiculos.API/SeguroVeiculos.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeguroVeiculos.API.dll"]

