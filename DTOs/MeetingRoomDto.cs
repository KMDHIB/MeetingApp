namespace MeetingApp.DTOs;

/// <summary>
/// Data Transfer Object for mødelokale med alle detaljer
/// </summary>
public class MeetingRoomDto
{
    public int Id { get; set; }
    public string Navn { get; set; } = string.Empty;
    public string Lokation { get; set; } = string.Empty;
    public int Pladsantal { get; set; }
}
