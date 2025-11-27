namespace MeetingApp.DTOs;

/// <summary>
/// Forenklet DTO til listevisning med kun navn og lokation
/// </summary>
public class MeetingRoomListDto
{
    public int Id { get; set; }
    public string Navn { get; set; } = string.Empty;
    public string Lokation { get; set; } = string.Empty;
}
