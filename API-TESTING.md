# Meeting Room API - Test Examples

Dette dokument indeholder eksempler på hvordan du kan teste API'en.

## Forudsætning
Sørg for at applikationen kører på http://localhost:5000

## Health Check

```bash
curl http://localhost:5000/api/health
```

Expected response:
```json
{
  "status": "Healthy",
  "message": "API og database kører",
  "database": "Connected",
  "meetingRoomsCount": 3,
  "timestamp": "2024-11-27T12:00:00Z",
  "version": "1.0.0"
}
```

## Hent alle mødelokaler (Liste)

```bash
curl http://localhost:5000/api/meetingrooms
```

Expected response:
```json
[
  {
    "id": 1,
    "navn": "Konferencesal A",
    "lokation": "1. sal, bygning Nord"
  },
  {
    "id": 2,
    "navn": "Meeting Room B",
    "lokation": "Stueetagen, bygning Syd"
  },
  {
    "id": 3,
    "navn": "Kreativt Rum",
    "lokation": "2. sal, bygning Vest"
  }
]
```

## Hent specifikt mødelokale (Detaljer)

```bash
curl http://localhost:5000/api/meetingrooms/1
```

Expected response:
```json
{
  "id": 1,
  "navn": "Konferencesal A",
  "lokation": "1. sal, bygning Nord",
  "pladsantal": 20
}
```

## Opret nyt mødelokale

```bash
curl -X POST http://localhost:5000/api/meetingrooms \
  -H "Content-Type: application/json" \
  -d '{
    "navn": "Innovation Hub",
    "lokation": "4. sal, bygning Center",
    "pladsantal": 30
  }'
```

Expected response (201 Created):
```json
{
  "id": 4,
  "navn": "Innovation Hub",
  "lokation": "4. sal, bygning Center",
  "pladsantal": 30
}
```

## Opdater mødelokale

```bash
curl -X PUT http://localhost:5000/api/meetingrooms/1 \
  -H "Content-Type: application/json" \
  -d '{
    "navn": "Konferencesal A (Opdateret)",
    "lokation": "1. sal, bygning Nord (Renoveret)",
    "pladsantal": 25
  }'
```

Expected response (200 OK):
```json
{
  "id": 1,
  "navn": "Konferencesal A (Opdateret)",
  "lokation": "1. sal, bygning Nord (Renoveret)",
  "pladsantal": 25
}
```

## Slet mødelokale

```bash
curl -X DELETE http://localhost:5000/api/meetingrooms/3
```

Expected response: 204 No Content

## Valideringsfejl eksempler

### Manglende påkrævet felt:
```bash
curl -X POST http://localhost:5000/api/meetingrooms \
  -H "Content-Type: application/json" \
  -d '{
    "lokation": "Test",
    "pladsantal": 10
  }'
```

Response (400 Bad Request):
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Navn": ["Navn er påkrævet"]
  }
}
```

### Ugyldig pladsantal:
```bash
curl -X POST http://localhost:5000/api/meetingrooms \
  -H "Content-Type: application/json" \
  -d '{
    "navn": "Test",
    "lokation": "Test",
    "pladsantal": 0
  }'
```

Response (400 Bad Request):
```json
{
  "errors": {
    "Pladsantal": ["Pladsantal skal være mellem 1 og 1000"]
  }
}
```

## Test med PowerShell (Windows)

```powershell
# Hent alle mødelokaler
Invoke-RestMethod -Uri "http://localhost:5000/api/meetingrooms" -Method Get

# Opret nyt mødelokale
$body = @{
    navn = "Test Lokale"
    lokation = "Test Location"
    pladsantal = 15
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/meetingrooms" `
  -Method Post `
  -Body $body `
  -ContentType "application/json"

# Hent specifikt mødelokale
Invoke-RestMethod -Uri "http://localhost:5000/api/meetingrooms/1" -Method Get

# Slet mødelokale
Invoke-RestMethod -Uri "http://localhost:5000/api/meetingrooms/1" -Method Delete
```

## Brug Swagger UI

Den nemmeste måde at teste API'en på er via Swagger UI:

1. Åbn http://localhost:5000 i din browser
2. Udvid en endpoint (f.eks. GET /api/meetingrooms)
3. Klik "Try it out"
4. Klik "Execute"
5. Se response nedenfor

Swagger UI giver dig også mulighed for at:
- Se detaljeret API dokumentation
- Teste alle endpoints direkte i browseren
- Se request/response schemas
- Downloade OpenAPI specifikation
