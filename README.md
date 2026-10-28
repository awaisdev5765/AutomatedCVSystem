# AutomatedCVSystem - Production Ready Documentation

## 📋 Table of Contents
- [Overview](#overview)
- [Prerequisites](#prerequisites)
- [Project Structure](#project-structure)
- [Installation & Setup](#installation--setup)
- [Database Configuration](#database-configuration)
- [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)
- [Configuration Files](#configuration-files)
- [Production Deployment](#production-deployment)
- [Troubleshooting](#troubleshooting)

## 🎯 Overview

The AutomatedCVSystem is a comprehensive .NET 7.0 application that provides:
- **User Profile Management** - Create and manage professional profiles
- **Job Analysis** - Analyze job descriptions and extract requirements
- **CV Generation** - Generate professional CVs in PDF and Word formats
- **Matching Algorithm** - Calculate compatibility scores between profiles and jobs

## 🔧 Prerequisites

### Required Software
- **.NET 7.0 SDK** (Download from [Microsoft](https://dotnet.microsoft.com/download/dotnet/7.0))
- **SQL Server** (LocalDB, Express, or Full version)
- **Visual Studio 2022** or **VS Code** (recommended)
- **Git** (for version control)

### Verify Installation
```bash
dotnet --version
# Should output: 7.0.x
```

## 📁 Project Structure

```
AutomatedCVSystem/
├── AutomatedCVSystem.API/           # Web API Project
│   ├── Controllers/                 # API Controllers
│   ├── Middleware/                  # Custom Middleware
│   ├── Program.cs                   # Application Entry Point
│   ├── appsettings.json            # Development Configuration
│   └── appsettings.Production.json # Production Configuration
├── AutomatedCVSystem.Core/          # Business Logic & Models
│   ├── DTOs/                       # Data Transfer Objects
│   ├── Interfaces/                 # Service Interfaces
│   └── Models/                     # Domain Models
├── AutomatedCVSystem.Infrastructure/ # Data Access & Services
│   ├── Data/                       # Database Context
│   ├── Services/                   # Business Services
│   └── Migrations/                 # Database Migrations
├── AutomatedCVSystem.Web/          # Blazor Web Application
│   ├── Pages/                      # Blazor Pages
│   ├── Services/                   # Client Services
│   └── wwwroot/                    # Static Files
└── AutomatedCVSystem.sln           # Solution File
```

## 🚀 Installation & Setup

### 1. Clone the Repository
```bash
git clone https://github.com/awaisdev5765/AutomatedCVSystem.git
cd AutomatedCVSystem
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Build the Solution
```bash
dotnet build
```

## 🗄️ Database Configuration

### 1. Update Connection String
Edit `AutomatedCVSystem.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AutomatedCVSystemDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 2. Create Database
```bash
# Navigate to API project directory
cd AutomatedCVSystem.API

# Create and apply migrations
dotnet ef database update --project ../AutomatedCVSystem.Infrastructure
```

### 3. Alternative: Using Package Manager Console
In Visual Studio, open Package Manager Console and run:
```powershell
Update-Database -Project AutomatedCVSystem.Infrastructure -StartupProject AutomatedCVSystem.API
```

## 🏃‍♂️ Running the Application

### Option 1: Run All Projects (Recommended)
```bash
# From solution root directory
dotnet run --project AutomatedCVSystem.API
```

### Option 2: Run Individual Projects

#### API Project (Backend)
```bash
cd AutomatedCVSystem.API
dotnet run
```
- **URL:** https://localhost:7001
- **Swagger UI:** https://localhost:7001/swagger

#### Web Project (Frontend)
```bash
cd AutomatedCVSystem.Web
dotnet run
```
- **URL:** https://localhost:7002

### Option 3: Using Visual Studio
1. Open `AutomatedCVSystem.sln` in Visual Studio
2. Set multiple startup projects:
   - Right-click solution → Properties
   - Set both API and Web projects to "Start"
3. Press F5 or click "Start"

## 📚 API Documentation

### Base URL
- **Development:** `https://localhost:7001/api`
- **Production:** `https://yourdomain.com/api`

### Main Endpoints

#### User Profiles
```
GET    /api/userprofiles              # Get all profiles
GET    /api/userprofiles/{id}         # Get profile by ID
POST   /api/userprofiles              # Create new profile
PUT    /api/userprofiles/{id}         # Update profile
DELETE /api/userprofiles/{id}         # Delete profile
```

#### CV Generation
```
POST   /api/cvgeneration/generate     # Generate CV
POST   /api/cvgeneration/generate-pdf # Generate PDF directly
POST   /api/cvgeneration/generate-word # Generate Word directly
GET    /api/cvgeneration/templates    # Get available templates
GET    /api/cvgeneration/user/{id}    # Get user's generated CVs
```

#### Job Analysis
```
POST   /api/jobanalysis/analyze/{userId}           # Analyze job description
POST   /api/jobanalysis/extract-skills             # Extract skills
POST   /api/jobanalysis/calculate-matching-score/{userId} # Calculate matching
```

### Sample API Calls

#### Create User Profile
```bash
curl -X POST "https://localhost:7001/api/userprofiles" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@example.com",
    "phoneNumber": "+1234567890",
    "professionalSummary": "Experienced software developer"
  }'
```

#### Generate CV
```bash
curl -X POST "https://localhost:7001/api/cvgeneration/generate" \
  -H "Content-Type: application/json" \
  -d '{
    "userProfileId": 1,
    "jobDescriptionId": 1,
    "templateType": "Professional",
    "fileFormat": "PDF",
    "highlightedSkills": ["C#", "ASP.NET"],
    "highlightedExperience": ["Microsoft"]
  }'
```

## ⚙️ Configuration Files

### Development Configuration (`appsettings.json`)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AutomatedCVSystemDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "AllowedHosts": "*",
  "AllowedOrigins": "https://localhost:7001,http://localhost:5000",
  "CVOutputPath": "GeneratedCVs",
  "JwtSettings": {
    "SecretKey": "development-secret-key-not-for-production-use",
    "Issuer": "AutomatedCVSystem",
    "Audience": "AutomatedCVSystem-Users",
    "ExpirationMinutes": 60
  }
}
```

### Production Configuration (`appsettings.Production.json`)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=AutomatedCVSystemDb;User Id=app_user;Password=secure_password;TrustServerCertificate=true;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "AllowedHosts": "yourdomain.com,www.yourdomain.com",
  "AllowedOrigins": "https://yourdomain.com,https://www.yourdomain.com",
  "CVOutputPath": "/app/generated-cvs",
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-here-must-be-at-least-32-characters-long",
    "Issuer": "AutomatedCVSystem",
    "Audience": "AutomatedCVSystem-Users",
    "ExpirationMinutes": 60
  }
}
```

## 🚀 Production Deployment

### 1. Environment Setup
```bash
# Set environment to production
export ASPNETCORE_ENVIRONMENT=Production

# Or on Windows
set ASPNETCORE_ENVIRONMENT=Production
```

### 2. Build for Production
```bash
dotnet publish -c Release -o ./publish
```

### 3. Docker Deployment (Optional)
Create `Dockerfile`:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

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

### 4. IIS Deployment
1. Install ASP.NET Core Hosting Bundle
2. Create application pool (.NET CLR Version: No Managed Code)
3. Deploy published files to IIS directory
4. Configure connection strings in `web.config`

## 🔧 Troubleshooting

### Common Issues

#### 1. Build Errors
```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

#### 2. Database Connection Issues
- Verify SQL Server is running
- Check connection string format
- Ensure database exists

#### 3. Port Conflicts
```bash
# Check what's using the port
netstat -ano | findstr :7001

# Kill process if needed
taskkill /PID <process_id> /F
```

#### 4. Missing Dependencies
```bash
# Restore packages
dotnet restore

# Clear NuGet cache
dotnet nuget locals all --clear
```

#### 5. File Generation Issues
- Ensure `GeneratedCVs` directory exists and is writable
- Check file permissions
- Verify QuestPDF and Xceed.Words.NET packages are installed

### Logs Location
- **Development:** Console output
- **Production:** Check application logs or configure external logging

### Performance Optimization
- Use connection pooling for database
- Implement caching for frequently accessed data
- Consider using CDN for static files
- Monitor memory usage for large file generations

## 📞 Support

For issues or questions:
1. Check the troubleshooting section above
2. Review the API documentation at `/swagger`
3. Check application logs for detailed error messages
4. Ensure all prerequisites are installed correctly

## 🔒 Security Notes

- **Never commit** production connection strings or secrets
- Use environment variables for sensitive configuration
- Regularly update dependencies for security patches
- Implement proper authentication before production use
- Use HTTPS in production environments

---

**Last Updated:** December 2024  
**Version:** 1.0.0  
**Framework:** .NET 7.0
