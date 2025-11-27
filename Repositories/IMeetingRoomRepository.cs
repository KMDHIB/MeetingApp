using MeetingApp.Models;

namespace MeetingApp.Repositories;

/// <summary>
/// Interface for mødelokale repository
/// </summary>
public interface IMeetingRoomRepository
{
    Task<IEnumerable<MeetingRoom>> GetAllAsync();
    Task<MeetingRoom?> GetByIdAsync(int id);
    Task<MeetingRoom> CreateAsync(MeetingRoom meetingRoom);
    Task<MeetingRoom?> UpdateAsync(int id, MeetingRoom meetingRoom);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
