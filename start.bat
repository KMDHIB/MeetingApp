@echo off
REM Meeting App - Quick Start Script for Windows
REM Dette script hjælper med at starte applikationen hurtigt

echo.
echo 🚀 Meeting App - Quick Start
echo ==============================
echo.

REM Check if Docker is running
docker info >nul 2>&1
if errorlevel 1 (
    echo ❌ Docker er ikke installeret eller kører ikke.
    echo    Start Docker Desktop og prøv igen.
    exit /b 1
)

echo ✅ Docker er installeret og kører
echo.

REM Stop existing containers if running
echo 🛑 Stopper eventuelle kørende containers...
docker-compose down 2>nul
echo.

REM Start the application
echo 🏗️  Bygger og starter applikationen...
docker-compose up --build -d
echo.

echo ⏳ Venter på at tjenesterne starter op...
timeout /t 5 /nobreak >nul
echo.

echo ✅ Applikationen kører nu!
echo.
echo 📍 Adgang til tjenesterne:
echo    • Swagger API:        http://localhost:5000
echo    • Adminer (Database): http://localhost:8080
echo.
echo 🔑 Database login til Adminer:
echo    System:   PostgreSQL
echo    Server:   db
echo    Username: postgres
echo    Password: postgres
echo    Database: meetingapp
echo.
echo 📝 Se logs med: docker-compose logs -f
echo 🛑 Stop med:    docker-compose down
echo.
