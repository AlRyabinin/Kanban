FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

# Создаём папку для SQLite
RUN mkdir -p /app/data

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["KanbanFlow.API/KanbanFlow.API.csproj", "KanbanFlow.API/"]
COPY ["KanbanFlow.Application/KanbanFlow.Application.csproj", "KanbanFlow.Application/"]
COPY ["KanbanFlow.Domain/KanbanFlow.Domain.csproj", "KanbanFlow.Domain/"]
COPY ["KanbanFlow.Infrastructure/KanbanFlow.Infrastructure.csproj", "KanbanFlow.Infrastructure/"]
RUN dotnet restore "./KanbanFlow.API/KanbanFlow.API.csproj"
COPY . .
WORKDIR "/src/KanbanFlow.API"
RUN dotnet build "./KanbanFlow.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./KanbanFlow.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KanbanFlow.API.dll"]