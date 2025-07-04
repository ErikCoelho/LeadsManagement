# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["LeadsManagement.Api/LeadsManagement.Api.csproj", "LeadsManagement.Api/"]
COPY ["LeadsManagement.Domain/LeadsManagement.Domain.csproj", "LeadsManagement.Domain/"]
COPY ["LeadsManagement.Infra/LeadsManagement.Infra.csproj", "LeadsManagement.Infra/"]
RUN dotnet restore "LeadsManagement.Api/LeadsManagement.Api.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/LeadsManagement.Api"
RUN dotnet build "LeadsManagement.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "LeadsManagement.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy published application
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Create non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "LeadsManagement.Api.dll"] 