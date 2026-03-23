# 1. Use the modern SDK (No "core" in the URL)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 2. Copy project files from both folders to satisfy dependencies
COPY ["worker/inventory-system-api.worker.csproj", "worker/"]
COPY ["inventory-system-api/inventory-system-api.csproj", "inventory-system-api/"]

# Ensure the json files are copied from the source to the app folder
COPY ["worker/appsettings.json", "./"]

# 3. Restore specifically for the worker
RUN dotnet restore "worker/inventory-system-api.worker.csproj"

# 4. Copy the rest of the source code
COPY . .

# 5. Build and Publish from the worker directory
WORKDIR "/src/worker"
RUN dotnet publish "inventory-system-api.worker.csproj" -c Release -o /app/out

# 6. Use the Runtime image (Smaller and faster than the SDK)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# 7. Start the worker
ENTRYPOINT ["dotnet", "inventory-system-api.worker.dll"]