using System.ComponentModel.DataAnnotations;

namespace MeetingApp.DTOs;

/// <summary>
/// DTO til oprettelse af nyt mødelokale
/// </summary>
public class CreateMeetingRoomDto
{
    [Required(ErrorMessage = "Navn er påkrævet")]
    [MaxLength(100, ErrorMessage = "Navn må ikke overstige 100 tegn")]
    public string Navn { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lokation er påkrævet")]
    [MaxLength(200, ErrorMessage = "Lokation må ikke overstige 200 tegn")]
    public string Lokation { get; set; } = string.Empty;

    [Range(1, 1000, ErrorMessage = "Pladsantal skal være mellem 1 og 1000")]
    public int Pladsantal { get; set; }
}
