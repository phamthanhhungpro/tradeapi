# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["trade.API/trade.API.csproj", "trade.API/"]
COPY ["trade.Logic/trade.Logic/trade.Logic.csproj", "trade.Logic/"]
COPY ["trade.InfraModel/trade.InfraModel/trade.InfraModel.csproj", "trade.InfraModel/"]
RUN dotnet restore "trade.API/trade.API.csproj"

# Copy remaining files and build
COPY . .
WORKDIR "/src/trade.API"
RUN dotnet build "trade.API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "trade.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "trade.API.dll"]
