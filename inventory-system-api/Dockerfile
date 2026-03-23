FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Copy the project files from their subfolders
# Format: COPY ["SourcePathFromRoot", "DestinationPathInDocker/"]
COPY ["inventory-system-api/inventory-system-api.csproj", "inventory-system-api/"]
COPY ["worker/inventory-system-api.worker.csproj", "worker/"]

# 2. Run restore on the API specifically
RUN dotnet restore "inventory-system-api/inventory-system-api.csproj"

# 3. Copy everything else (this includes all your .cs files)
COPY . .

# 4. Change directory to the API folder to build it
WORKDIR "/src/inventory-system-api"
RUN dotnet publish "inventory-system-api.csproj" -c Release -o /app/publish

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "inventory-system-api.dll"]