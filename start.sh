#!/bin/bash

# Meeting App - Quick Start Script
# Dette script hjælper med at starte applikationen hurtigt

set -e

echo "🚀 Meeting App - Quick Start"
echo "=============================="
echo ""

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "❌ Docker er ikke installeret. Installer Docker først."
    exit 1
fi

# Check if Docker Compose is installed
if ! command -v docker-compose &> /dev/null; then
    echo "❌ Docker Compose er ikke installeret. Installer Docker Compose først."
    exit 1
fi

echo "✅ Docker og Docker Compose er installeret"
echo ""

# Stop existing containers if running
echo "🛑 Stopper eventuelle kørende containers..."
docker-compose down 2>/dev/null || true
echo ""

# Start the application
echo "🏗️  Bygger og starter applikationen..."
docker-compose up --build -d
echo ""

echo "⏳ Venter på at tjenesterne starter op..."
sleep 5

# Check if services are running
if docker-compose ps | grep -q "Up"; then
    echo ""
    echo "✅ Applikationen kører nu!"
    echo ""
    echo "📍 Adgang til tjenesterne:"
    echo "   • Swagger API:        http://localhost:5000"
    echo "   • Adminer (Database): http://localhost:8080"
    echo ""
    echo "🔑 Database login til Adminer:"
    echo "   System:   PostgreSQL"
    echo "   Server:   db"
    echo "   Username: postgres"
    echo "   Password: postgres"
    echo "   Database: meetingapp"
    echo ""
    echo "📝 Se logs med: docker-compose logs -f"
    echo "🛑 Stop med:    docker-compose down"
    echo ""
else
    echo ""
    echo "❌ Der opstod en fejl. Tjek logs med: docker-compose logs"
    exit 1
fi
