using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MeetingApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeetingRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Navn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Lokation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Pladsantal = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingRooms", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MeetingRooms",
                columns: new[] { "Id", "Lokation", "Navn", "Pladsantal" },
                values: new object[,]
                {
                    { 1, "1. sal, bygning Nord", "Konferencesal A", 20 },
                    { 2, "Stueetagen, bygning Syd", "Meeting Room B", 8 },
                    { 3, "2. sal, bygning Vest", "Kreativt Rum", 12 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeetingRooms");
        }
    }
}
