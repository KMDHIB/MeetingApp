using System.ComponentModel.DataAnnotations;

namespace MeetingApp.Models;

/// <summary>
/// Repræsenterer et mødelokale
/// </summary>
public class MeetingRoom
{
    /// <summary>
    /// Unik identifikator for mødelokalet
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Navnet på mødelokalet
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Navn { get; set; } = string.Empty;

    /// <summary>
    /// Lokationen hvor mødelokalet befinder sig
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Lokation { get; set; } = string.Empty;

    /// <summary>
    /// Antal pladser i mødelokalet
    /// </summary>
    [Range(1, 1000)]
    public int Pladsantal { get; set; }
}
