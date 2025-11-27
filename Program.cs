using Microsoft.EntityFrameworkCore;
using MeetingApp.Data;
using MeetingApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Tilføj services til containeren
builder.Services.AddControllers();

// Database configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Repository pattern - demonstrerer dependency injection og clean architecture
builder.Services.AddScoped<IMeetingRoomRepository, MeetingRoomRepository>();

// Swagger/OpenAPI konfiguration for API dokumentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Meeting Room API",
        Version = "v1",
        Description = "API til administration af mødelokaler - CRUD operationer for mødelokaler med navn, lokation og pladsantal",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Meeting App Team"
        }
    });

    // Inkluder XML kommentarer i Swagger dokumentationen
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// CORS - tillad alle origins i development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Kør database migrations og seed data automatisk ved opstart
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Sikrer at databasen er oprettet og migrations er kørt
        context.Database.Migrate();
        
        app.Logger.LogInformation("Database migrations completed successfully");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while migrating the database");
    }
}

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Meeting Room API v1");
        c.RoutePrefix = string.Empty; // Swagger UI som root
    });
}

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Root endpoint - redirecter til Swagger
app.MapGet("/", () => Results.Redirect("/index.html"))
    .ExcludeFromDescription();

// Vis en venlig besked om hvor Swagger findes
app.Logger.LogInformation("Meeting Room API is running! Access Swagger UI at: http://localhost:5000");

app.Run();
