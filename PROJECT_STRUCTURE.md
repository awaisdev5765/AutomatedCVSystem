# 📁 Project Structure Guide

## 🎯 Overview
This document provides a detailed breakdown of the AutomatedCVSystem project structure, explaining what each file and folder does.

## 📂 Root Directory Structure

```
AutomatedCVSystem/
├── 📄 AutomatedCVSystem.sln          # Visual Studio Solution File
├── 📄 README.md                      # Main Documentation
├── 📄 QUICKSTART.md                  # Quick Start Guide
├── 📄 DEPLOYMENT.md                  # Production Deployment Guide
├── 📄 PROJECT_STRUCTURE.md           # This File
├── 📁 scripts/                       # Setup Scripts
│   ├── setup.sh                      # Linux/macOS Setup Script
│   └── setup.bat                     # Windows Setup Script
├── 📁 AutomatedCVSystem.API/         # Web API Project
├── 📁 AutomatedCVSystem.Core/        # Business Logic Layer
├── 📁 AutomatedCVSystem.Infrastructure/ # Data Access Layer
└── 📁 AutomatedCVSystem.Web/         # Blazor Web Application
```

## 🏗️ Project Architecture

### 1. AutomatedCVSystem.API (Web API Layer)
**Purpose:** RESTful API endpoints and HTTP handling

```
AutomatedCVSystem.API/
├── 📁 Controllers/                   # API Controllers
│   ├── CVGenerationController.cs     # CV generation endpoints
│   ├── JobAnalysisController.cs      # Job analysis endpoints
│   ├── UserProfilesController.cs     # User profile management
│   └── WeatherForecastController.cs  # Default template controller
├── 📁 Middleware/                    # Custom Middleware
│   └── GlobalExceptionMiddleware.cs  # Global error handling
├── 📁 Properties/                    # Project Properties
│   └── launchSettings.json          # Launch configuration
├── 📄 Program.cs                     # Application entry point
├── 📄 appsettings.json              # Development configuration
├── 📄 appsettings.Production.json   # Production configuration
└── 📄 AutomatedCVSystem.API.csproj  # Project file
```

**Key Files:**
- **`Program.cs`** - Main application startup, service registration, middleware configuration
- **`Controllers/`** - REST API endpoints for all functionality
- **`appsettings.json`** - Development configuration (connection strings, logging, etc.)
- **`appsettings.Production.json`** - Production-specific settings

### 2. AutomatedCVSystem.Core (Business Logic Layer)
**Purpose:** Domain models, DTOs, and business interfaces

```
AutomatedCVSystem.Core/
├── 📁 DTOs/                         # Data Transfer Objects
│   ├── CVGenerationDto.cs           # CV generation request/response models
│   ├── JobAnalysisDto.cs            # Job analysis models
│   └── UserProfileDto.cs            # User profile models
├── 📁 Interfaces/                   # Service Interfaces
│   ├── ICVGenerationService.cs      # CV generation service contract
│   ├── IJobAnalysisService.cs       # Job analysis service contract
│   └── IUserProfileService.cs       # User profile service contract
├── 📁 Models/                       # Domain Models
│   ├── Certification.cs             # Certification entity
│   ├── Education.cs                 # Education entity
│   ├── GeneratedCV.cs               # Generated CV entity
│   ├── JobDescription.cs            # Job description entity
│   ├── Project.cs                   # Project entity
│   ├── Skill.cs                     # Skill entity
│   ├── UserProfile.cs               # User profile entity
│   └── WorkExperience.cs            # Work experience entity
└── 📄 AutomatedCVSystem.Core.csproj # Project file
```

**Key Files:**
- **`Models/`** - Database entities with validation attributes
- **`DTOs/`** - Data transfer objects for API communication
- **`Interfaces/`** - Service contracts for dependency injection

### 3. AutomatedCVSystem.Infrastructure (Data Access Layer)
**Purpose:** Database access, external services, and business logic implementation

```
AutomatedCVSystem.Infrastructure/
├── 📁 Data/                         # Database Context
│   └── ApplicationDbContext.cs      # Entity Framework DbContext
├── 📁 Migrations/                   # Database Migrations
│   ├── 20241216000000_InitialCreate.cs # Initial database schema
│   └── ApplicationDbContextModelSnapshot.cs # Migration snapshot
├── 📁 Services/                     # Business Services
│   ├── CVGenerationService.cs       # CV generation implementation
│   ├── JobAnalysisService.cs        # Job analysis implementation
│   └── UserProfileService.cs        # User profile management
└── 📄 AutomatedCVSystem.Infrastructure.csproj # Project file
```

**Key Files:**
- **`ApplicationDbContext.cs`** - Entity Framework database context
- **`Services/`** - Implementation of business logic services
- **`Migrations/`** - Database schema versioning

### 4. AutomatedCVSystem.Web (Blazor Web Application)
**Purpose:** User interface and client-side functionality

```
AutomatedCVSystem.Web/
├── 📁 Pages/                        # Blazor Pages
│   ├── CVGeneration.razor           # CV generation page
│   ├── JobAnalysis.razor            # Job analysis page
│   ├── Profiles.razor               # User profiles page
│   ├── Index.razor                  # Home page
│   └── _Host.cshtml                 # Host page
├── 📁 Services/                     # Client Services
│   └── ApiService.cs                # API communication service
├── 📁 Shared/                       # Shared Components
│   ├── MainLayout.razor             # Main layout component
│   ├── NavMenu.razor                # Navigation menu
│   └── SurveyPrompt.razor           # Survey component
├── 📁 wwwroot/                      # Static Files
│   ├── 📁 css/                      # Stylesheets
│   ├── 📁 js/                       # JavaScript files
│   └── favicon.png                  # Site icon
├── 📄 Program.cs                    # Web app entry point
└── 📄 AutomatedCVSystem.Web.csproj  # Project file
```

**Key Files:**
- **`Pages/`** - Blazor pages for different features
- **`Services/ApiService.cs`** - HTTP client for API communication
- **`wwwroot/`** - Static files (CSS, JS, images)

## 🔧 Configuration Files

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

## 🚀 How to Run Different Parts

### Run API Only (Backend)
```bash
cd AutomatedCVSystem.API
dotnet run
# Access at: https://localhost:7001
# Swagger UI: https://localhost:7001/swagger
```

### Run Web App Only (Frontend)
```bash
cd AutomatedCVSystem.Web
dotnet run
# Access at: https://localhost:7002
```

### Run Both (Full Application)
```bash
# Terminal 1 - API
dotnet run --project AutomatedCVSystem.API

# Terminal 2 - Web App
dotnet run --project AutomatedCVSystem.Web
```

### Database Operations
```bash
# Create/Update Database
cd AutomatedCVSystem.API
dotnet ef database update --project ../AutomatedCVSystem.Infrastructure

# Add New Migration
dotnet ef migrations add MigrationName --project ../AutomatedCVSystem.Infrastructure
```

## 📊 Data Flow

```
User Interface (Blazor) 
    ↓ HTTP Requests
API Controllers 
    ↓ Service Calls
Business Services (Infrastructure)
    ↓ Database Operations
Entity Framework DbContext
    ↓ SQL Queries
SQL Server Database
```

## 🔍 Key Dependencies

### API Project Dependencies
- `Microsoft.AspNetCore.OpenApi` - OpenAPI/Swagger support
- `Microsoft.EntityFrameworkCore.SqlServer` - SQL Server provider
- `Microsoft.AspNetCore.Authentication.JwtBearer` - JWT authentication
- `Swashbuckle.AspNetCore` - Swagger UI

### Infrastructure Project Dependencies
- `Microsoft.EntityFrameworkCore` - Entity Framework Core
- `QuestPDF` - PDF generation
- `Xceed.Words.NET` - Word document generation
- `Microsoft.ML` - Machine learning for job analysis

### Core Project Dependencies
- No external dependencies (pure business logic)

### Web Project Dependencies
- `Microsoft.AspNetCore.Components.Web` - Blazor Server
- References to Core project for DTOs

## 🛠️ Development Workflow

1. **Models** - Define in `Core/Models/`
2. **DTOs** - Create in `Core/DTOs/`
3. **Interfaces** - Define in `Core/Interfaces/`
4. **Services** - Implement in `Infrastructure/Services/`
5. **Controllers** - Create in `API/Controllers/`
6. **Pages** - Build in `Web/Pages/`
7. **Database** - Migrate using EF Core commands

## 📝 File Naming Conventions

- **Controllers:** `[Feature]Controller.cs`
- **Services:** `[Feature]Service.cs`
- **Models:** `[Entity].cs`
- **DTOs:** `[Feature]Dto.cs`
- **Pages:** `[Feature].razor`
- **Configuration:** `appsettings.[Environment].json`

## 🔧 Common Commands

```bash
# Build entire solution
dotnet build

# Run specific project
dotnet run --project AutomatedCVSystem.API

# Add new package
dotnet add package PackageName

# Create new migration
dotnet ef migrations add MigrationName --project AutomatedCVSystem.Infrastructure

# Update database
dotnet ef database update --project AutomatedCVSystem.Infrastructure

# Clean solution
dotnet clean

# Restore packages
dotnet restore
```

---

This structure follows Clean Architecture principles with clear separation of concerns, making the codebase maintainable and testable.
