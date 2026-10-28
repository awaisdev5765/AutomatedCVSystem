#!/bin/bash

# AutomatedCVSystem Setup Script
# This script helps you get the project running quickly

echo "🚀 AutomatedCVSystem Setup Script"
echo "=================================="

# Check if .NET 7.0 is installed
echo "📋 Checking prerequisites..."
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK not found. Please install .NET 7.0 SDK first."
    echo "   Download from: https://dotnet.microsoft.com/download/dotnet/7.0"
    exit 1
fi

DOTNET_VERSION=$(dotnet --version)
echo "✅ .NET SDK found: $DOTNET_VERSION"

if [[ ! $DOTNET_VERSION == 7.* ]]; then
    echo "⚠️  Warning: .NET 7.0 is required. Current version: $DOTNET_VERSION"
    echo "   Please install .NET 7.0 SDK"
    exit 1
fi

# Navigate to project root
cd "$(dirname "$0")/.."

echo ""
echo "🔧 Setting up the project..."

# Restore packages
echo "📦 Restoring NuGet packages..."
dotnet restore
if [ $? -ne 0 ]; then
    echo "❌ Failed to restore packages"
    exit 1
fi

# Build the solution
echo "🔨 Building the solution..."
dotnet build
if [ $? -ne 0 ]; then
    echo "❌ Build failed"
    exit 1
fi

echo "✅ Build successful!"

# Check if database exists
echo ""
echo "🗄️  Database setup..."
echo "Do you want to create/update the database? (y/n)"
read -r response
if [[ "$response" =~ ^([yY][eE][sS]|[yY])$ ]]; then
    echo "Creating/updating database..."
    cd AutomatedCVSystem.API
    dotnet ef database update --project ../AutomatedCVSystem.Infrastructure
    if [ $? -eq 0 ]; then
        echo "✅ Database setup complete!"
    else
        echo "❌ Database setup failed. Please check your connection string."
    fi
    cd ..
fi

# Create GeneratedCVs directory
echo ""
echo "📁 Creating output directories..."
mkdir -p GeneratedCVs
echo "✅ GeneratedCVs directory created"

# Ask if user wants to run the application
echo ""
echo "🏃‍♂️ Ready to run the application!"
echo "Choose an option:"
echo "1) Run API only (Backend)"
echo "2) Run Web app only (Frontend)"
echo "3) Run both (requires multiple terminals)"
echo "4) Exit"
read -r choice

case $choice in
    1)
        echo "🚀 Starting API..."
        dotnet run --project AutomatedCVSystem.API
        ;;
    2)
        echo "🚀 Starting Web app..."
        dotnet run --project AutomatedCVSystem.Web
        ;;
    3)
        echo "🚀 Starting both applications..."
        echo "API will run on: https://localhost:7001"
        echo "Web app will run on: https://localhost:7002"
        echo ""
        echo "Opening API in new terminal..."
        if command -v gnome-terminal &> /dev/null; then
            gnome-terminal -- dotnet run --project AutomatedCVSystem.API
        elif command -v xterm &> /dev/null; then
            xterm -e "dotnet run --project AutomatedCVSystem.API" &
        else
            echo "Please run 'dotnet run --project AutomatedCVSystem.API' in a new terminal"
        fi
        
        echo "Starting Web app in current terminal..."
        dotnet run --project AutomatedCVSystem.Web
        ;;
    4)
        echo "👋 Setup complete! Run the following commands when ready:"
        echo "   API:    dotnet run --project AutomatedCVSystem.API"
        echo "   Web:    dotnet run --project AutomatedCVSystem.Web"
        echo ""
        echo "📚 Documentation:"
        echo "   - Quick Start: QUICKSTART.md"
        echo "   - Full Guide: README.md"
        echo "   - Deployment: DEPLOYMENT.md"
        ;;
    *)
        echo "Invalid choice. Exiting..."
        ;;
esac

echo ""
echo "🎉 Setup complete!"
echo "📚 Check the documentation files for more information:"
echo "   - QUICKSTART.md - Quick start guide"
echo "   - README.md - Full documentation"
echo "   - DEPLOYMENT.md - Production deployment guide"
