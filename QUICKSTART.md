# 🚀 Quick Start Guide - AutomatedCVSystem

## ⚡ Get Running in 5 Minutes

### 1. Prerequisites Check
```bash
# Verify .NET 7.0 is installed
dotnet --version
# Should show: 7.0.x
```

### 2. Build & Run
```bash
# From the project root directory
cd /Users/airtechsolutions/AutomatedCVSystem

# Restore packages
dotnet restore

# Build the solution
dotnet build

# Run the API (Backend)
dotnet run --project AutomatedCVSystem.API
```

### 3. Access the Application
- **API (Backend):** https://localhost:7001
- **Swagger Documentation:** https://localhost:7001/swagger
- **Web App (Frontend):** https://localhost:7002

### 4. Database Setup (First Time Only)
```bash
# Create the database
cd AutomatedCVSystem.API
dotnet ef database update --project ../AutomatedCVSystem.Infrastructure
```

## 🎯 Key Files to Know

| File | Purpose | Location |
|------|---------|----------|
| **Main Entry Point** | API startup | `AutomatedCVSystem.API/Program.cs` |
| **Database Config** | Connection strings | `AutomatedCVSystem.API/appsettings.json` |
| **API Controllers** | REST endpoints | `AutomatedCVSystem.API/Controllers/` |
| **Business Logic** | Core services | `AutomatedCVSystem.Infrastructure/Services/` |
| **Data Models** | Database entities | `AutomatedCVSystem.Core/Models/` |
| **Web Interface** | Blazor pages | `AutomatedCVSystem.Web/Pages/` |

## 🔧 Quick Commands

```bash
# Run API only
dotnet run --project AutomatedCVSystem.API

# Run Web app only  
dotnet run --project AutomatedCVSystem.Web

# Run both (set multiple startup projects in Visual Studio)

# Update database
dotnet ef database update --project AutomatedCVSystem.Infrastructure --startup-project AutomatedCVSystem.API

# Clean and rebuild
dotnet clean && dotnet restore && dotnet build
```

## 📋 First Steps After Running

1. **Open Swagger UI:** https://localhost:7001/swagger
2. **Create a User Profile** using the `/api/userprofiles` endpoint
3. **Test CV Generation** using the `/api/cvgeneration/generate` endpoint
4. **Access Web Interface** at https://localhost:7002

## 🚨 If Something Goes Wrong

```bash
# Check if ports are free
netstat -ano | findstr :7001
netstat -ano | findstr :7002

# Kill processes if needed
taskkill /PID <process_id> /F

# Clean everything and start fresh
dotnet clean
dotnet restore
dotnet build
```

## 📁 Generated Files Location
- **CVs:** `GeneratedCVs/` folder in the API project directory
- **Logs:** Console output (development) or configured logging (production)

---
**Need more details?** Check the full [README.md](README.md) for comprehensive documentation.
