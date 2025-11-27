using Microsoft.AspNetCore.Mvc;
using MeetingApp.DTOs;
using MeetingApp.Models;
using MeetingApp.Repositories;

namespace MeetingApp.Controllers;

/// <summary>
/// API Controller til administration af mødelokaler
/// Implementerer CRUD operationer (Create, Read, Update, Delete)
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MeetingRoomsController : ControllerBase
{
    private readonly IMeetingRoomRepository _repository;
    private readonly ILogger<MeetingRoomsController> _logger;

    public MeetingRoomsController(
        IMeetingRoomRepository repository,
        ILogger<MeetingRoomsController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Hent alle mødelokaler som liste (kun navn og lokation)
    /// </summary>
    /// <returns>Liste af mødelokaler med grundlæggende information</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MeetingRoomListDto>>> GetAll()
    {
        _logger.LogInformation("Henter alle mødelokaler");
        
        var meetingRooms = await _repository.GetAllAsync();
        var listDtos = meetingRooms.Select(m => new MeetingRoomListDto
        {
            Id = m.Id,
            Navn = m.Navn,
            Lokation = m.Lokation
        });

        return Ok(listDtos);
    }

    /// <summary>
    /// Hent detaljeret information om et specifikt mødelokale
    /// </summary>
    /// <param name="id">ID på mødelokalet</param>
    /// <returns>Detaljeret information om mødelokalet</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MeetingRoomDto>> GetById(int id)
    {
        _logger.LogInformation("Henter mødelokale med ID: {Id}", id);
        
        var meetingRoom = await _repository.GetByIdAsync(id);
        if (meetingRoom == null)
        {
            _logger.LogWarning("Mødelokale med ID {Id} blev ikke fundet", id);
            return NotFound(new { message = $"Mødelokale med ID {id} blev ikke fundet" });
        }

        var dto = new MeetingRoomDto
        {
            Id = meetingRoom.Id,
            Navn = meetingRoom.Navn,
            Lokation = meetingRoom.Lokation,
            Pladsantal = meetingRoom.Pladsantal
        };

        return Ok(dto);
    }

    /// <summary>
    /// Opret et nyt mødelokale
    /// </summary>
    /// <param name="createDto">Data for det nye mødelokale</param>
    /// <returns>Det nyoprettede mødelokale</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MeetingRoomDto>> Create([FromBody] CreateMeetingRoomDto createDto)
    {
        _logger.LogInformation("Opretter nyt mødelokale: {Navn}", createDto.Navn);

        var meetingRoom = new MeetingRoom
        {
            Navn = createDto.Navn,
            Lokation = createDto.Lokation,
            Pladsantal = createDto.Pladsantal
        };

        var createdRoom = await _repository.CreateAsync(meetingRoom);

        var dto = new MeetingRoomDto
        {
            Id = createdRoom.Id,
            Navn = createdRoom.Navn,
            Lokation = createdRoom.Lokation,
            Pladsantal = createdRoom.Pladsantal
        };

        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    /// <summary>
    /// Opdater et eksisterende mødelokale
    /// </summary>
    /// <param name="id">ID på mødelokalet der skal opdateres</param>
    /// <param name="updateDto">Opdaterede data</param>
    /// <returns>Det opdaterede mødelokale</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MeetingRoomDto>> Update(int id, [FromBody] UpdateMeetingRoomDto updateDto)
    {
        _logger.LogInformation("Opdaterer mødelokale med ID: {Id}", id);

        var meetingRoom = new MeetingRoom
        {
            Navn = updateDto.Navn,
            Lokation = updateDto.Lokation,
            Pladsantal = updateDto.Pladsantal
        };

        var updatedRoom = await _repository.UpdateAsync(id, meetingRoom);
        if (updatedRoom == null)
        {
            _logger.LogWarning("Mødelokale med ID {Id} blev ikke fundet", id);
            return NotFound(new { message = $"Mødelokale med ID {id} blev ikke fundet" });
        }

        var dto = new MeetingRoomDto
        {
            Id = updatedRoom.Id,
            Navn = updatedRoom.Navn,
            Lokation = updatedRoom.Lokation,
            Pladsantal = updatedRoom.Pladsantal
        };

        return Ok(dto);
    }

    /// <summary>
    /// Slet et mødelokale
    /// </summary>
    /// <param name="id">ID på mødelokalet der skal slettes</param>
    /// <returns>Status for sletningen</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Sletter mødelokale med ID: {Id}", id);

        var deleted = await _repository.DeleteAsync(id);
        if (!deleted)
        {
            _logger.LogWarning("Mødelokale med ID {Id} blev ikke fundet", id);
            return NotFound(new { message = $"Mødelokale med ID {id} blev ikke fundet" });
        }

        return NoContent();
    }
}
