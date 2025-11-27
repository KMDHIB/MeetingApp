# ⚡ Quick Reference - Meeting App

## 🚀 Start Application

```bash
docker-compose up
```

## 🌐 Access Points

| Service | URL | Description |
|---------|-----|-------------|
| **API / Swagger** | http://localhost:5000 | Interactive API documentation |
| **Adminer** | http://localhost:8080 | Database management UI |
| **Health Check** | http://localhost:5000/api/health | API status |

## 🔑 Database Credentials

```
System:   PostgreSQL
Server:   db
Username: postgres
Password: postgres
Database: meetingapp
```

## 📡 API Endpoints

### List All Meeting Rooms
```bash
GET /api/meetingrooms
```

### Get Meeting Room Details
```bash
GET /api/meetingrooms/{id}
```

### Create Meeting Room
```bash
POST /api/meetingrooms
Content-Type: application/json

{
  "navn": "Mødelokale navn",
  "lokation": "Lokation",
  "pladsantal": 10
}
```

### Update Meeting Room
```bash
PUT /api/meetingrooms/{id}
Content-Type: application/json

{
  "navn": "Opdateret navn",
  "lokation": "Ny lokation",
  "pladsantal": 15
}
```

### Delete Meeting Room
```bash
DELETE /api/meetingrooms/{id}
```

## 🛠️ Docker Commands

```bash
# Start all services
docker-compose up

# Start in background
docker-compose up -d

# Rebuild and start
docker-compose up --build

# View logs
docker-compose logs -f

# Stop all services
docker-compose down

# Stop and remove volumes
docker-compose down -v
```

## 🧪 Quick Test

```bash
# Health check
curl http://localhost:5000/api/health

# Get all meeting rooms
curl http://localhost:5000/api/meetingrooms

# Get specific meeting room
curl http://localhost:5000/api/meetingrooms/1

# Create new meeting room
curl -X POST http://localhost:5000/api/meetingrooms \
  -H "Content-Type: application/json" \
  -d '{"navn":"Test Room","lokation":"Test Location","pladsantal":10}'
```

## 📚 Documentation

- **README.md** - Full getting started guide
- **ARCHITECTURE.md** - Technical deep-dive
- **API-TESTING.md** - Complete testing examples
- **SUMMARY.md** - Implementation overview

## ⚙️ Development

```bash
# Restore packages
dotnet restore

# Build project
dotnet build

# Run locally (requires PostgreSQL)
dotnet run

# Create new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update
```

## 🐛 Troubleshooting

### Port already in use
```bash
# Change ports in docker-compose.yml
ports:
  - "5001:5000"  # API
  - "8081:8080"  # Adminer
```

### Database connection issues
```bash
# Check database is running
docker-compose ps

# Restart services
docker-compose restart

# View database logs
docker-compose logs db
```

### Reset everything
```bash
# Stop and remove all
docker-compose down -v

# Start fresh
docker-compose up --build
```

## 📊 Project Statistics

- **Language:** C# (.NET 8)
- **Database:** PostgreSQL 16
- **API Endpoints:** 6
- **Database Tables:** 1
- **Seed Data:** 3 meeting rooms
- **Docker Services:** 3 (API, Database, Adminer)

## 🎯 Features

✅ Full CRUD operations  
✅ Repository pattern  
✅ Async/await throughout  
✅ Input validation  
✅ Swagger documentation  
✅ Health checks  
✅ Auto migrations  
✅ Database UI tool  
✅ Docker ready  
✅ Production ready  

---

**Need help?** Check the full documentation in README.md
