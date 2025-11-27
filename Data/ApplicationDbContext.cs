using Microsoft.EntityFrameworkCore;
using MeetingApp.Models;

namespace MeetingApp.Data;

/// <summary>
/// Database context for mødelokale applikationen
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<MeetingRoom> MeetingRooms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed nogle eksempler på mødelokaler
        modelBuilder.Entity<MeetingRoom>().HasData(
            new MeetingRoom
            {
                Id = 1,
                Navn = "Konferencesal A",
                Lokation = "1. sal, bygning Nord",
                Pladsantal = 20
            },
            new MeetingRoom
            {
                Id = 2,
                Navn = "Meeting Room B",
                Lokation = "Stueetagen, bygning Syd",
                Pladsantal = 8
            },
            new MeetingRoom
            {
                Id = 3,
                Navn = "Kreativt Rum",
                Lokation = "2. sal, bygning Vest",
                Pladsantal = 12
            }
        );
    }
}
