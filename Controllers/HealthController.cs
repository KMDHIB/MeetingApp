using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeetingApp.Data;

namespace MeetingApp.Controllers;

/// <summary>
/// Health check endpoint til at verificere at API'en og databasen kører
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HealthController> _logger;

    public HealthController(ApplicationDbContext context, ILogger<HealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Simpel health check der verificerer API og database connectivity
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Get()
    {
        try
        {
            // Test database connectivity
            var canConnect = await _context.Database.CanConnectAsync();
            
            if (!canConnect)
            {
                _logger.LogError("Database connection failed");
                return StatusCode(503, new
                {
                    status = "Unhealthy",
                    message = "Kunne ikke forbinde til databasen",
                    timestamp = DateTime.UtcNow
                });
            }

            var meetingRoomCount = await _context.MeetingRooms.CountAsync();

            return Ok(new
            {
                status = "Healthy",
                message = "API og database kører",
                database = "Connected",
                meetingRoomsCount = meetingRoomCount,
                timestamp = DateTime.UtcNow,
                version = "1.0.0"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");
            return StatusCode(503, new
            {
                status = "Unhealthy",
                message = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
}
