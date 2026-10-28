@echo off
REM AutomatedCVSystem Setup Script for Windows
REM This script helps you get the project running quickly

echo 🚀 AutomatedCVSystem Setup Script
echo ==================================

REM Check if .NET 7.0 is installed
echo 📋 Checking prerequisites...
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ❌ .NET SDK not found. Please install .NET 7.0 SDK first.
    echo    Download from: https://dotnet.microsoft.com/download/dotnet/7.0
    pause
    exit /b 1
)

for /f "tokens=*" %%i in ('dotnet --version') do set DOTNET_VERSION=%%i
echo ✅ .NET SDK found: %DOTNET_VERSION%

echo %DOTNET_VERSION% | findstr /r "^7\." >nul
if %errorlevel% neq 0 (
    echo ⚠️  Warning: .NET 7.0 is required. Current version: %DOTNET_VERSION%
    echo    Please install .NET 7.0 SDK
    pause
    exit /b 1
)

REM Navigate to project root
cd /d "%~dp0.."

echo.
echo 🔧 Setting up the project...

REM Restore packages
echo 📦 Restoring NuGet packages...
dotnet restore
if %errorlevel% neq 0 (
    echo ❌ Failed to restore packages
    pause
    exit /b 1
)

REM Build the solution
echo 🔨 Building the solution...
dotnet build
if %errorlevel% neq 0 (
    echo ❌ Build failed
    pause
    exit /b 1
)

echo ✅ Build successful!

REM Check if database exists
echo.
echo 🗄️  Database setup...
set /p response="Do you want to create/update the database? (y/n): "
if /i "%response%"=="y" (
    echo Creating/updating database...
    cd AutomatedCVSystem.API
    dotnet ef database update --project ../AutomatedCVSystem.Infrastructure
    if %errorlevel% equ 0 (
        echo ✅ Database setup complete!
    ) else (
        echo ❌ Database setup failed. Please check your connection string.
    )
    cd ..
)

REM Create GeneratedCVs directory
echo.
echo 📁 Creating output directories...
if not exist "GeneratedCVs" mkdir GeneratedCVs
echo ✅ GeneratedCVs directory created

REM Ask if user wants to run the application
echo.
echo 🏃‍♂️ Ready to run the application!
echo Choose an option:
echo 1) Run API only (Backend)
echo 2) Run Web app only (Frontend)
echo 3) Run both (requires multiple command windows)
echo 4) Exit
set /p choice="Enter your choice (1-4): "

if "%choice%"=="1" (
    echo 🚀 Starting API...
    dotnet run --project AutomatedCVSystem.API
) else if "%choice%"=="2" (
    echo 🚀 Starting Web app...
    dotnet run --project AutomatedCVSystem.Web
) else if "%choice%"=="3" (
    echo 🚀 Starting both applications...
    echo API will run on: https://localhost:7001
    echo Web app will run on: https://localhost:7002
    echo.
    echo Starting API in new window...
    start "AutomatedCVSystem API" cmd /k "dotnet run --project AutomatedCVSystem.API"
    echo Starting Web app in current window...
    dotnet run --project AutomatedCVSystem.Web
) else if "%choice%"=="4" (
    echo 👋 Setup complete! Run the following commands when ready:
    echo    API:    dotnet run --project AutomatedCVSystem.API
    echo    Web:    dotnet run --project AutomatedCVSystem.Web
    echo.
    echo 📚 Documentation:
    echo    - Quick Start: QUICKSTART.md
    echo    - Full Guide: README.md
    echo    - Deployment: DEPLOYMENT.md
) else (
    echo Invalid choice. Exiting...
)

echo.
echo 🎉 Setup complete!
echo 📚 Check the documentation files for more information:
echo    - QUICKSTART.md - Quick start guide
echo    - README.md - Full documentation
echo    - DEPLOYMENT.md - Production deployment guide

pause
