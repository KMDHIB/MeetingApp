using Microsoft.EntityFrameworkCore;
using MeetingApp.Data;
using MeetingApp.Models;

namespace MeetingApp.Repositories;

/// <summary>
/// Repository implementation for mødelokaler med fokus på performance og clean code
/// </summary>
public class MeetingRoomRepository : IMeetingRoomRepository
{
    private readonly ApplicationDbContext _context;

    public MeetingRoomRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MeetingRoom>> GetAllAsync()
    {
        return await _context.MeetingRooms
            .AsNoTracking()
            .OrderBy(m => m.Navn)
            .ToListAsync();
    }

    public async Task<MeetingRoom?> GetByIdAsync(int id)
    {
        return await _context.MeetingRooms
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<MeetingRoom> CreateAsync(MeetingRoom meetingRoom)
    {
        _context.MeetingRooms.Add(meetingRoom);
        await _context.SaveChangesAsync();
        return meetingRoom;
    }

    public async Task<MeetingRoom?> UpdateAsync(int id, MeetingRoom meetingRoom)
    {
        var existingRoom = await _context.MeetingRooms.FindAsync(id);
        if (existingRoom == null)
        {
            return null;
        }

        existingRoom.Navn = meetingRoom.Navn;
        existingRoom.Lokation = meetingRoom.Lokation;
        existingRoom.Pladsantal = meetingRoom.Pladsantal;

        await _context.SaveChangesAsync();
        return existingRoom;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var meetingRoom = await _context.MeetingRooms.FindAsync(id);
        if (meetingRoom == null)
        {
            return false;
        }

        _context.MeetingRooms.Remove(meetingRoom);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.MeetingRooms.AnyAsync(m => m.Id == id);
    }
}
