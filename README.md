# Meeting App

En moderne .NET 8 Web API til administration af mødelokaler med PostgreSQL database, Docker support og Swagger dokumentation.

## 📋 Opgavebeskrivelse

* Din opgave er at implementere en applikation hvori man kan oprette, redigere, slette og læse mødelokaler. Løsningen skal både inkludere en detalje læsning ud fra lokalets ID samt en listevisning der inkluderer navn og lokation.
* Et mødelokale har felterne Id, Navn, Lokation og Pladsantal.
* Du bestemmer selv om du vil lave en web applikation, nogle REST services, en konsol applikation eller en kombination, da det ikke er brugergrænsefladen der er det vigtige.
* Du skal anvende følgende teknologier:
	* En relationel database som f.eks PostgreSQL, MySQL eller Maria DB. Dog er det et krav at databasen har et Docker image (se det sidste punkt).
	* C# / .NET Core
	* Docker (Der skal laves en Docker fil som kan bygge og køre projektet) 
* Du skal lave en Docker Compose fil som kan starte projektet. Den skal bygge Docker imaget for applikationen og linke det til en instans af databasen hvorefter applikationen skal startes op og være klar til brug. En bonus vil være også at inkluderer et eller andet UI værktøj til at browse databasens indhold i docker compose filen. I så fald vil kommandoen docker-compose up starte alle afhængighederne for din applikation, så du kan demonstrere den.
* Det er mere vigtigt at du viser noget fedt med C#/.NET end at du demonstrerer at du kan anvende Visual Studio til at auto generere Razor views, data access logik el.lign.
* Vær kreativ - gode idéer er velkomne.

## 🚀 Kom i gang

### Forudsætninger

- Docker og Docker Compose installeret på dit system
- (Valgfrit) .NET 8 SDK hvis du vil køre applikationen lokalt uden Docker

### Start applikationen med Docker Compose

Den nemmeste måde at køre hele applikationen:

```bash
docker-compose up
```

Dette starter:
- **PostgreSQL database** på port 5432
- **.NET Web API** på port 5000
- **Adminer** (database UI) på port 8080

### Adgang til applikationen

Efter `docker-compose up` er kørt, kan du tilgå:

- **Swagger API dokumentation**: http://localhost:5000
- **Adminer (Database UI)**: http://localhost:8080
  - System: PostgreSQL
  - Server: db
  - Username: postgres
  - Password: postgres
  - Database: meetingapp

## 📖 API Endpoints

Alle endpoints er tilgængelige via Swagger UI på http://localhost:5000

### GET /api/meetingrooms
Hent liste af alle mødelokaler (navn og lokation)

**Response:**
```json
[
  {
    "id": 1,
    "navn": "Konferencesal A",
    "lokation": "1. sal, bygning Nord"
  }
]
```

### GET /api/meetingrooms/{id}
Hent detaljeret information om et specifikt mødelokale

**Response:**
```json
{
  "id": 1,
  "navn": "Konferencesal A",
  "lokation": "1. sal, bygning Nord",
  "pladsantal": 20
}
```

### POST /api/meetingrooms
Opret et nyt mødelokale

**Request Body:**
```json
{
  "navn": "Nyt Mødelokale",
  "lokation": "3. sal, bygning Øst",
  "pladsantal": 15
}
```

### PUT /api/meetingrooms/{id}
Opdater et eksisterende mødelokale

**Request Body:**
```json
{
  "navn": "Opdateret Navn",
  "lokation": "Ny Lokation",
  "pladsantal": 25
}
```

### DELETE /api/meetingrooms/{id}
Slet et mødelokale

## 🏗️ Arkitektur & Design

### Teknologier
- **.NET 8** - Moderne C# web framework
- **PostgreSQL** - Relationel database
- **Entity Framework Core** - ORM med Code-First migrations
- **Swagger/OpenAPI** - API dokumentation
- **Docker** - Containerization
- **Adminer** - Database management UI

### Design Patterns & Best Practices

✅ **Repository Pattern** - Adskiller data access logik fra business logik
- `IMeetingRoomRepository` interface for testbarhed
- Clean separation of concerns

✅ **Dependency Injection** - Moderne .NET DI container
- Scoped services for repositories
- DbContext lifecycle management

✅ **DTO Pattern** - Data Transfer Objects
- Separate DTOs for Create, Update, List og Detail views
- Validering med Data Annotations
- Dansk navngivning for forretningsdomænet

✅ **Async/Await** - Asynkron programmering
- Alle database operationer er async
- Bedre skalerbarhed og resource udnyttelse

✅ **Entity Framework Migrations** - Database version control
- Code-First approach
- Automatisk migration ved opstart
- Seed data inkluderet

✅ **Docker Best Practices**
- Multi-stage build for mindre images
- Non-root user for sikkerhed
- Health checks
- Optimeret layer caching

✅ **Logging** - Struktureret logging
- ILogger dependency injection
- Logging af vigtige operationer

## 📁 Projektstruktur

```
MeetingApp/
├── Controllers/          # API controllers
│   └── MeetingRoomsController.cs
├── Data/                 # Database context
│   └── ApplicationDbContext.cs
├── DTOs/                 # Data Transfer Objects
│   ├── CreateMeetingRoomDto.cs
│   ├── UpdateMeetingRoomDto.cs
│   ├── MeetingRoomDto.cs
│   └── MeetingRoomListDto.cs
├── Models/               # Domain entities
│   └── MeetingRoom.cs
├── Repositories/         # Data access layer
│   ├── IMeetingRoomRepository.cs
│   └── MeetingRoomRepository.cs
├── Migrations/           # EF Core migrations
├── Dockerfile            # Multi-stage Docker build
├── docker-compose.yml    # Orchestration
└── Program.cs            # Application startup
```

## 🧪 Test API'en med curl

```bash
# Hent alle mødelokaler
curl http://localhost:5000/api/meetingrooms

# Hent specifikt mødelokale
curl http://localhost:5000/api/meetingrooms/1

# Opret nyt mødelokale
curl -X POST http://localhost:5000/api/meetingrooms \
  -H "Content-Type: application/json" \
  -d '{
    "navn": "Test Lokale",
    "lokation": "Test Location",
    "pladsantal": 10
  }'

# Opdater mødelokale
curl -X PUT http://localhost:5000/api/meetingrooms/1 \
  -H "Content-Type: application/json" \
  -d '{
    "navn": "Opdateret Navn",
    "lokation": "Ny Lokation",
    "pladsantal": 25
  }'

# Slet mødelokale
curl -X DELETE http://localhost:5000/api/meetingrooms/1
```

## 🛠️ Udvikling

### Kør lokalt uden Docker

```bash
# Restore dependencies
dotnet restore

# Kør migrations (kræver PostgreSQL kørende)
dotnet ef database update

# Start applikationen
dotnet run
```

### Opret ny migration

```bash
dotnet ef migrations add MigrationName
```

### Stop alle services

```bash
docker-compose down
```

### Genopbyg images

```bash
docker-compose up --build
```

## 🎯 Features & Highlights

- ✨ **Clean Architecture** - Separation of concerns med repositories og DTOs
- 🔒 **Input Validation** - Data annotations og model validation
- 📝 **API Documentation** - Komplet Swagger/OpenAPI spec med XML kommentarer
- 🐳 **Production-ready Docker** - Multi-stage builds og health checks
- 🗄️ **Database UI** - Adminer til nem database browsing
- 🌱 **Seed Data** - Inkluderer eksempel mødelokaler
- 🔄 **Auto Migrations** - Database migrations køres automatisk ved opstart
- 📊 **Structured Logging** - ILogger integration
- 🌐 **CORS Enabled** - Klar til frontend integration
- 🎨 **Dansk Domænemodel** - Autentisk dansk terminologi i hele domænet

## Aflevering

* Aflever gerne en delvis løsning, hvis du ikke kan lave den helt færdig af den ene eller anden grund.
* Du kan vælge at forke dette repo og sende løsningen ind som et Pull Request eller alternativt at zippe løsningen og sende den som mail (adresse udleveres seperat)

