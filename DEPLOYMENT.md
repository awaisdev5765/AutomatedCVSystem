# 🚀 Production Deployment Guide

## 📋 Pre-Deployment Checklist

- [ ] .NET 7.0 SDK installed on production server
- [ ] SQL Server configured and accessible
- [ ] Production database created
- [ ] Environment variables configured
- [ ] SSL certificates ready
- [ ] File storage configured
- [ ] Monitoring/logging setup

## 🏗️ Deployment Steps

### 1. Environment Configuration

#### Set Environment Variables
```bash
# Linux/macOS
export ASPNETCORE_ENVIRONMENT=Production
export ConnectionStrings__DefaultConnection="Server=prod-server;Database=AutomatedCVSystemDb;User Id=app_user;Password=secure_password;TrustServerCertificate=true;MultipleActiveResultSets=true"

# Windows
set ASPNETCORE_ENVIRONMENT=Production
set ConnectionStrings__DefaultConnection=Server=prod-server;Database=AutomatedCVSystemDb;User Id=app_user;Password=secure_password;TrustServerCertificate=true;MultipleActiveResultSets=true
```

#### Update Production Configuration
Edit `appsettings.Production.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_PRODUCTION_SERVER;Database=AutomatedCVSystemDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=true;MultipleActiveResultSets=true"
  },
  "AllowedHosts": "yourdomain.com,www.yourdomain.com",
  "AllowedOrigins": "https://yourdomain.com,https://www.yourdomain.com",
  "CVOutputPath": "/app/generated-cvs",
  "JwtSettings": {
    "SecretKey": "YOUR_SUPER_SECRET_KEY_AT_LEAST_32_CHARACTERS_LONG",
    "Issuer": "AutomatedCVSystem",
    "Audience": "AutomatedCVSystem-Users",
    "ExpirationMinutes": 60
  }
}
```

### 2. Build for Production

```bash
# Clean previous builds
dotnet clean

# Restore packages
dotnet restore

# Build in Release mode
dotnet build -c Release

# Publish for production
dotnet publish -c Release -o ./publish
```

### 3. Database Migration

```bash
# Apply migrations to production database
dotnet ef database update --project AutomatedCVSystem.Infrastructure --startup-project AutomatedCVSystem.API --environment Production
```

### 4. File System Setup

```bash
# Create directories for file storage
mkdir -p /app/generated-cvs
chmod 755 /app/generated-cvs

# Ensure proper permissions
chown -R www-data:www-data /app/generated-cvs
```

## 🐳 Docker Deployment

### Create Dockerfile
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Create directory for generated files
RUN mkdir -p /app/generated-cvs
RUN chmod 755 /app/generated-cvs

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AutomatedCVSystem.API/AutomatedCVSystem.API.csproj", "AutomatedCVSystem.API/"]
COPY ["AutomatedCVSystem.Core/AutomatedCVSystem.Core.csproj", "AutomatedCVSystem.Core/"]
COPY ["AutomatedCVSystem.Infrastructure/AutomatedCVSystem.Infrastructure.csproj", "AutomatedCVSystem.Infrastructure/"]
RUN dotnet restore "AutomatedCVSystem.API/AutomatedCVSystem.API.csproj"
COPY . .
WORKDIR "/src/AutomatedCVSystem.API"
RUN dotnet build "AutomatedCVSystem.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AutomatedCVSystem.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AutomatedCVSystem.API.dll"]
```

### Docker Commands
```bash
# Build Docker image
docker build -t automatedcvsystem .

# Run container
docker run -d \
  --name automatedcvsystem \
  -p 80:80 \
  -p 443:443 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ConnectionStrings__DefaultConnection="Server=host.docker.internal;Database=AutomatedCVSystemDb;User Id=app_user;Password=secure_password;TrustServerCertificate=true;MultipleActiveResultSets=true" \
  -v /host/path/generated-cvs:/app/generated-cvs \
  automatedcvsystem
```

## 🌐 IIS Deployment

### 1. Install Prerequisites
- ASP.NET Core Hosting Bundle 7.0
- IIS with ASP.NET Core Module

### 2. Create Application Pool
- .NET CLR Version: No Managed Code
- Process Model Identity: ApplicationPoolIdentity
- Enable 32-bit Applications: False

### 3. Deploy Application
```bash
# Publish to folder
dotnet publish -c Release -o C:\inetpub\wwwroot\AutomatedCVSystem

# Copy to IIS directory
xcopy C:\path\to\publish C:\inetpub\wwwroot\AutomatedCVSystem /E /I
```

### 4. Configure web.config
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet" arguments=".\AutomatedCVSystem.API.dll" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" hostingModel="inprocess" />
    </system.webServer>
  </location>
</configuration>
```

## ☁️ Cloud Deployment Options

### Azure App Service
```bash
# Install Azure CLI
az login

# Create resource group
az group create --name AutomatedCVSystem-RG --location "East US"

# Create App Service plan
az appservice plan create --name AutomatedCVSystem-Plan --resource-group AutomatedCVSystem-RG --sku B1

# Create web app
az webapp create --resource-group AutomatedCVSystem-RG --plan AutomatedCVSystem-Plan --name your-app-name --runtime "DOTNET|7.0"

# Deploy
az webapp deployment source config-zip --resource-group AutomatedCVSystem-RG --name your-app-name --src publish.zip
```

### AWS Elastic Beanstalk
```bash
# Install EB CLI
pip install awsebcli

# Initialize EB application
eb init

# Create environment
eb create production

# Deploy
eb deploy
```

## 🔒 Security Configuration

### 1. SSL/TLS Setup
```bash
# Generate SSL certificate (Let's Encrypt)
certbot --nginx -d yourdomain.com -d www.yourdomain.com
```

### 2. Firewall Configuration
```bash
# Allow only necessary ports
ufw allow 80
ufw allow 443
ufw allow 22  # SSH
ufw enable
```

### 3. Environment Variables for Secrets
```bash
# Never hardcode secrets in configuration files
export JwtSettings__SecretKey="your-super-secret-key"
export ConnectionStrings__DefaultConnection="your-connection-string"
```

## 📊 Monitoring & Logging

### 1. Application Insights (Azure)
```json
{
  "ApplicationInsights": {
    "InstrumentationKey": "your-instrumentation-key"
  }
}
```

### 2. Health Checks
```csharp
// Add to Program.cs
builder.Services.AddHealthChecks();
app.MapHealthChecks("/health");
```

### 3. Logging Configuration
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    },
    "ApplicationInsights": {
      "LogLevel": {
        "Default": "Information"
      }
    }
  }
}
```

## 🔄 CI/CD Pipeline (GitHub Actions)

### .github/workflows/deploy.yml
```yaml
name: Deploy to Production

on:
  push:
    branches: [ main ]

jobs:
  deploy:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '7.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Test
      run: dotnet test --no-build --verbosity normal
    
    - name: Publish
      run: dotnet publish -c Release -o ./publish
    
    - name: Deploy to Azure
      uses: azure/webapps-deploy@v2
      with:
        app-name: 'your-app-name'
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ./publish
```

## 🚨 Troubleshooting Production Issues

### Common Issues

#### 1. Database Connection
```bash
# Test connection
dotnet ef database update --project AutomatedCVSystem.Infrastructure --startup-project AutomatedCVSystem.API --environment Production --verbose
```

#### 2. File Permissions
```bash
# Check permissions
ls -la /app/generated-cvs
chmod 755 /app/generated-cvs
chown www-data:www-data /app/generated-cvs
```

#### 3. Memory Issues
```bash
# Monitor memory usage
top -p $(pgrep -f AutomatedCVSystem.API)
```

#### 4. SSL Certificate Issues
```bash
# Check certificate
openssl s_client -connect yourdomain.com:443 -servername yourdomain.com
```

### Log Analysis
```bash
# View application logs
tail -f /var/log/automatedcvsystem/app.log

# Check system logs
journalctl -u automatedcvsystem -f
```

## 📈 Performance Optimization

### 1. Database Optimization
- Enable connection pooling
- Add database indexes
- Use read replicas for heavy queries

### 2. Caching
```csharp
// Add Redis caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "your-redis-connection-string";
});
```

### 3. CDN Setup
- Use CDN for static files
- Enable compression
- Implement proper caching headers

---

**Production Checklist:**
- [ ] Environment variables configured
- [ ] Database migrated and tested
- [ ] SSL certificates installed
- [ ] File storage configured
- [ ] Monitoring setup
- [ ] Backup strategy implemented
- [ ] Security headers configured
- [ ] Performance monitoring active
