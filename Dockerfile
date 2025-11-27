# Multi-stage Dockerfile for .NET 8 Application
# Optimeret til hurtigere builds og mindre image størrelse

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopier csproj og restore dependencies (caching layer)
COPY ["MeetingApp.csproj", "./"]
RUN dotnet restore "MeetingApp.csproj"

# Kopier resten af kildekoden
COPY . .

# Build applikationen
RUN dotnet build "MeetingApp.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "MeetingApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Opret non-root bruger for sikkerhed
RUN useradd -m -u 1000 appuser && chown -R appuser /app
USER appuser

# Kopier published applikation fra publish stage
COPY --from=publish /app/publish .

# Eksponér port 5000
EXPOSE 5000

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:5000/api/meetingrooms || exit 1

# Start applikationen
ENTRYPOINT ["dotnet", "MeetingApp.dll"]
