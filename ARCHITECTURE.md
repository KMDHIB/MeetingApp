# ARCHITECTURE.md

## Teknisk Arkitektur - Meeting Room API

Dette dokument beskriver de tekniske valg og arkitektur beslutninger i Meeting Room API'en.

## 🏛️ Arkitektur Oversigt

```
┌─────────────────────────────────────────────────────────┐
│                     Presentation Layer                   │
│  ┌───────────────────────────────────────────────────┐  │
│  │  MeetingRoomsController + HealthController         │  │
│  │  (HTTP Endpoints, Validation, Response mapping)   │  │
│  └───────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────┐
│                    Data Transfer Layer                   │
│  ┌───────────────────────────────────────────────────┐  │
│  │  DTOs (CreateMeetingRoomDto, UpdateMeetingRoomDto) │  │
│  │  (Data validation, transformation)                │  │
│  └───────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────┐
│                   Business Logic Layer                   │
│  ┌───────────────────────────────────────────────────┐  │
│  │  IMeetingRoomRepository (Interface)               │  │
│  │  MeetingRoomRepository (Implementation)           │  │
│  │  (Business rules, data access orchestration)     │  │
│  └───────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────┐
│                    Data Access Layer                     │
│  ┌───────────────────────────────────────────────────┐  │
│  │  ApplicationDbContext (EF Core)                   │  │
│  │  MeetingRoom Entity                               │  │
│  │  (ORM, migrations, database operations)           │  │
│  └───────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
                           │
                           ▼
                  ┌────────────────┐
                  │   PostgreSQL   │
                  │    Database    │
                  └────────────────┘
```

## 🎨 Design Patterns

### 1. Repository Pattern
**Hvorfor?**
- Adskiller data access logik fra business logik
- Gør det nemmere at teste (mock repositories)
- Centraliserer database queries
- SOLID princippet (Single Responsibility)

**Implementation:**
```csharp
public interface IMeetingRoomRepository
{
    Task<IEnumerable<MeetingRoom>> GetAllAsync();
    Task<MeetingRoom?> GetByIdAsync(int id);
    // ... etc
}
```

### 2. Data Transfer Object (DTO) Pattern
**Hvorfor?**
- Beskytter domain modellen
- Adskiller interne og eksterne data strukturer
- Validering på input niveau
- Forskellige views (List vs Detail)

**Implementation:**
- `CreateMeetingRoomDto` - Input til POST
- `UpdateMeetingRoomDto` - Input til PUT
- `MeetingRoomDto` - Output med alle felter
- `MeetingRoomListDto` - Output kun med navn og lokation

### 3. Dependency Injection
**Hvorfor?**
- Loose coupling
- Testability
- Lifecycle management
- .NET Core native pattern

**Implementation:**
```csharp
builder.Services.AddScoped<IMeetingRoomRepository, MeetingRoomRepository>();
```

## 🔧 Tekniske Valg

### .NET 8
- **LTS version** med lang support
- **Performance improvements** over tidligere versioner
- **Minimal APIs** support (brugt til root redirect)
- **Native AOT ready** for fremtidig optimering

### Entity Framework Core
- **Code-First** approach - koden er sandhedskilden
- **Migrations** - versionsstyring af database schema
- **LINQ** - type-safe queries
- **Async/Await** - non-blocking operations

### PostgreSQL
- **Open source** og production-proven
- **ACID compliance**
- **Excellent Docker support**
- **JSON support** for fremtidig udvidelse
- **Better performance** end MySQL for komplekse queries

### Swagger/OpenAPI
- **Auto-generated documentation**
- **Interactive testing UI**
- **Client code generation** mulighed
- **Industry standard**

## 📊 Database Design

### MeetingRooms Table
```sql
CREATE TABLE "MeetingRooms" (
    "Id" SERIAL PRIMARY KEY,
    "Navn" VARCHAR(100) NOT NULL,
    "Lokation" VARCHAR(200) NOT NULL,
    "Pladsantal" INTEGER NOT NULL,
    CONSTRAINT "CK_MeetingRooms_Pladsantal" CHECK ("Pladsantal" >= 1 AND "Pladsantal" <= 1000)
);
```

**Design beslutninger:**
- `SERIAL` for auto-increment ID
- `VARCHAR` med passende længder for performance
- Check constraint på Pladsantal
- Danske navne for at matche domænet

## 🔒 Sikkerhed & Best Practices

### Input Validation
- Data Annotations på DTOs
- Model State validation i controllers
- Range validation på Pladsantal

### Docker Security
```dockerfile
RUN useradd -m -u 1000 appuser
USER appuser
```
- Non-root user i container
- Minimerer sikkerhedsrisiko

### Health Checks
- Database connectivity check
- Bruges af Docker HEALTHCHECK
- Monitoring ready

## 🚀 Performance Optimizationer

### 1. AsNoTracking()
```csharp
return await _context.MeetingRooms
    .AsNoTracking()
    .ToListAsync();
```
- Bruges til read-only queries
- Reducerer memory overhead
- Hurtigere query execution

### 2. Async/Await
```csharp
public async Task<MeetingRoom?> GetByIdAsync(int id)
{
    return await _context.MeetingRooms
        .AsNoTracking()
        .FirstOrDefaultAsync(m => m.Id == id);
}
```
- Non-blocking I/O
- Bedre skalering
- Flere samtidige requests

### 3. Docker Multi-Stage Build
```dockerfile
FROM sdk AS build
FROM build AS publish
FROM runtime AS final
```
- Mindre final image (runtime vs SDK)
- Hurtigere deployments
- Layer caching for builds

### 4. Database Connection Pooling
- Built-in i Npgsql
- Genbruger connections
- Reducerer overhead

## 🔄 CI/CD Overvejelser

### Container Orchestration
Applikationen er klar til:
- **Kubernetes** deployment
- **Docker Swarm**
- **Azure Container Apps**
- **AWS ECS/Fargate**

### Environment Configuration
- `appsettings.json` for defaults
- `appsettings.Development.json` for local dev
- Environment variables i Docker
- Secrets management ready

### Monitoring & Logging
- Structured logging med ILogger
- Log levels konfigurerbare
- Ready for Application Insights / Prometheus

## 📈 Skalerbarhed

### Horizontal Scaling
- **Stateless API** - kan køre multiple instances
- Database som shared state
- Load balancer ready

### Database Scaling
- PostgreSQL read replicas
- Connection pooling
- Index optimization muligheder

## 🧪 Testing Strategy

### Unit Tests (ikke implementeret, men ready for)
```csharp
public class MeetingRoomRepositoryTests
{
    private readonly Mock<ApplicationDbContext> _mockContext;
    private readonly IMeetingRoomRepository _repository;

    [Fact]
    public async Task GetAllAsync_ReturnsAllMeetingRooms()
    {
        // Arrange
        // Act
        var result = await _repository.GetAllAsync();
        // Assert
        Assert.NotNull(result);
    }
}
```

### Integration Tests
- Test API endpoints
- Test database migrations
- Test Docker build

## 🔮 Fremtidige Udvidelser

### Potentielle Features
1. **Authentication/Authorization**
   - JWT tokens
   - Role-based access control

2. **Booking System**
   - BookingRoom entity
   - Time slots
   - Conflict detection

3. **Search & Filtering**
   - Full-text search
   - Filter by capacity
   - Location-based search

4. **Caching**
   - Redis for frequently accessed data
   - Cache invalidation strategy

5. **Event Sourcing**
   - Audit log af alle ændringer
   - Event-driven architecture

### Database Migrationer
```bash
# Tilføj ny migration
dotnet ef migrations add AddBookingSystem

# Opdater database
dotnet ef database update
```

## 📚 Ressourcer

- [.NET 8 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Docker Best Practices](https://docs.docker.com/develop/dev-best-practices/)
- [RESTful API Guidelines](https://restfulapi.net/)

## 🤝 Bidrag

Se README.md for information om hvordan du kommer i gang med udvikling.
